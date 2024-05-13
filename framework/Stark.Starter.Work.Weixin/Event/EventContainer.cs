using System.Xml;
using Microsoft.Extensions.Options;
using Stark.Starter.Work.Weixin.Event.Events;
using Stark.Starter.Work.Weixin.Event.Messages;
using Stark.Starter.Work.Weixin.Models;
using Stark.Starter.Work.Weixin.Security;
using Volo.Abp.DependencyInjection;

namespace Stark.Starter.Work.Weixin.Event;

public class EventContainer : ITransientDependency
{
    /// <summary>
    /// 文本消息处理
    /// </summary>
    public event Func<TextMessage, Task<IEventAnswer>> TextMessageHandler;

    private readonly WorkWxConfig _workWxConfig;

    public EventContainer(IOptions<WorkWxConfig> config)
    {
        _workWxConfig = config.Value;
    }

    /// <summary>
    /// 验证微信Token配置
    /// </summary>
    /// <param name="signature"></param>
    /// <param name="timestamp"></param>
    /// <param name="nonce"></param>
    /// <param name="echostr"></param>
    /// <returns></returns>
    public string CheckSignature(string signature, string timestamp, string nonce, string echostr)
    {
        if (string.IsNullOrWhiteSpace(signature))
            return string.Empty;
        var wxcpt = new WXBizMsgCrypt(_workWxConfig.Token, _workWxConfig.EncodingAESKey, _workWxConfig.CorpID);

        string sEchoStr = "";
        var ret = wxcpt.VerifyURL(signature, timestamp, nonce, echostr, ref sEchoStr);
        if (ret != 0)
        {
            return string.Empty;
        }

        return sEchoStr;
    }

    public async Task<IEventAnswer> AnswerAsync(string? signature, string? timestamp, string? nonce, string postData)
    {
        var wxcpt = new WXBizMsgCrypt(_workWxConfig.Token, _workWxConfig.EncodingAESKey, _workWxConfig.CorpID);
        var eventData = ""; // 解析之后的明文
        var ret = wxcpt.DecryptMsg(signature, timestamp, nonce, postData, ref eventData);
        if (ret != 0)
        {
            return new EventAnswerSuccess();
        }

        var xmlDoc = new XmlDocument();

        try
        {
            Console.WriteLine("企业微信回调事件信息：" + eventData);
            xmlDoc.LoadXml(eventData);
        }
        catch (Exception)
        {
            return new EventAnswerEmpty();
        }

        var msgTypeNode = xmlDoc.SelectSingleNode("/xml/MsgType");

        if (msgTypeNode == null)
        {
            return new EventAnswerEmpty();
        }

        switch (msgTypeNode.InnerText)
        {
            case "event":
                return await HandleEventAsync(xmlDoc);
            case TextMessage.MsgType:
                return await HandleMessageAsync(xmlDoc);
            default:
                return new EventAnswerSuccess();
        }
    }


    // 处理事件
    private async Task<IEventAnswer> HandleEventAsync(XmlDocument xmlDoc)
    {
        var eventName = xmlDoc.SelectSingleNode("/xml/Event");
        Console.WriteLine("eventName :" + eventName);

        return new EventAnswerSuccess();
    }

    private async Task<IEventAnswer> HandleMessageAsync(XmlDocument xmlDoc)
    {
        var msgType = xmlDoc.SelectSingleNode("/xml/MsgType");
        if (TextMessage.MsgType.Equals(msgType.InnerText))
        {
            return await OnTextMessageAsync(xmlDoc);
        }

        return new EventAnswerSuccess();
    }


    private async Task<IEventAnswer> OnTextMessageAsync(XmlDocument xmlDoc)
    {
        if (TextMessageHandler == null)
        {
            throw new Exception("未绑定文本消息处理任务");
        }

        var e = CreateMessage<TextMessage>(xmlDoc);
        e.Context = xmlDoc.SelectSingleNode("/xml/Content")?.InnerText ?? "";

        var answer = await TextMessageHandler.Invoke(e);

        return answer;
    }

    private T CreateMessage<T>(XmlDocument xmlDoc) where T : MessageBase, new()
    {
        return new T
        {
            CreateTime = long.Parse(xmlDoc.SelectSingleNode("/xml/CreateTime")?.InnerText),
            FromUserName = xmlDoc.SelectSingleNode("/xml/FromUserName")?.InnerText,
            ToUserName = xmlDoc.SelectSingleNode("/xml/ToUserName")?.InnerText,
            MsgId = xmlDoc.SelectSingleNode("/xml/MsgId")?.InnerText,
            AgentID = xmlDoc.SelectSingleNode("/xml/AgentID")?.InnerText,
        };
    }

    private T CreateEvent<T>(XmlDocument xmlDoc) where T : EventBase, new()
    {
        return new T
        {
            CreateTime = long.Parse(xmlDoc.SelectSingleNode("/xml/CreateTime")?.InnerText),
            FromUserName = xmlDoc.SelectSingleNode("/xml/FromUserName")?.InnerText,
            ToUserName = xmlDoc.SelectSingleNode("/xml/ToUserName")?.InnerText,
        };
    }
}