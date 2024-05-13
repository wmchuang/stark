namespace Stark.Starter.Work.Weixin.Event.Messages;

public class MessageBase
{
    /// <summary>
    /// 企业微信CorpID
    /// </summary>
    public string ToUserName { get; set; }

    /// <summary>
    /// 成员UserID
    /// </summary>
    public string FromUserName { get; set; }

    /// <summary>
    /// 消息创建时间
    /// </summary>
    public long CreateTime { get; set; }

    /// <summary>
    /// 消息id，64位整型
    /// </summary>
    public string MsgId { get; set; }

    /// <summary>
    /// 企业应用的id，整型
    /// </summary>
    public string AgentID { get; set; }
}