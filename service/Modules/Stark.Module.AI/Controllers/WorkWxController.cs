using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stark.Module.AI.Backgrounds;
using Stark.Starter.Web.Attributes;
using Stark.Starter.Work.Weixin.Event;
using Stark.Starter.Work.Weixin.Event.Messages;

namespace Stark.Module.AI.Controllers;

/// <summary>
/// 企微回调
/// </summary>
[ApiController]
[Route("api/workWx/[action]")]
[DontWrapResult]
public class WorkWxController : ControllerBase
{
    private readonly EventContainer _eventContainer;

    public WorkWxController(EventContainer eventContainer)
    {
        _eventContainer = eventContainer;
        _eventContainer.TextMessageHandler += TextMessageHandler;
    }

    /// <summary>
    /// 验证/事件应答推送地址
    /// </summary>
    /// <returns></returns>
    [HttpGet, HttpPost]
    public async Task<IActionResult> Answer()
    {
        if (HttpMethods.IsGet(HttpContext.Request.Method))
            return HandleToken();
        else
            return await HandleEvent();
    }

    /// <summary>
    /// 文本消息处理
    /// </summary>
    /// <param name="arg"></param>
    /// <returns></returns>
    private async Task<IEventAnswer> TextMessageHandler(TextMessage arg)
    {
        await WorkWxBackgroundService.AddTaskAsync(arg);

        return new EventAnswerSuccess();
    }

    /// <summary>
    /// 企业微信推送事件处理
    /// </summary>
    /// <returns></returns>
    private async Task<IActionResult> HandleEvent()
    {
        string
            signature = HttpContext.Request
                .Query["msg_signature"]; //微信加密签名，msg_signature结合了企业填写的token、请求中的timestamp、nonce参数、加密的消息体
        string timestamp = HttpContext.Request.Query["timestamp"]; //时间戳
        string nonce = HttpContext.Request.Query["nonce"]; //随机数    
        var input = HttpContext.Request.Body;

        string postData;
        using (var reader = new StreamReader(input, Encoding.UTF8))
            postData = await reader.ReadToEndAsync();

        var eventAnswer = await _eventContainer.AnswerAsync(signature, timestamp, nonce, postData);
        return Content(eventAnswer.FormatString());
    }

    /// <summary>
    /// 验证企业微信Url
    /// </summary>
    /// <returns></returns>
    private IActionResult HandleToken()
    {
        string
            signature = HttpContext.Request
                .Query["msg_signature"]; //微信加密签名，msg_signature结合了企业填写的token、请求中的timestamp、nonce参数、加密的消息体
        string timestamp = HttpContext.Request.Query["timestamp"]; //时间戳
        string nonce = HttpContext.Request.Query["nonce"]; //随机数
        string
            echostr = HttpContext.Request
                .Query
                    ["echostr"]; //加密的随机字符串，以msg_encrypt格式提供。需要解密并返回echostr明文，解密后有random、msg_len、msg、$CorpID四个字段，其中msg即为echostr明文

        if (string.IsNullOrWhiteSpace(signature)
            || string.IsNullOrWhiteSpace(timestamp)
            || string.IsNullOrWhiteSpace(nonce)
            || string.IsNullOrWhiteSpace(echostr))
            return NotFound();

        var sEchoStr = _eventContainer.CheckSignature(signature, timestamp, nonce, echostr);
        if (!string.IsNullOrWhiteSpace(sEchoStr))
        {
            return Content(sEchoStr);
        }

        return NoContent();
    }
}