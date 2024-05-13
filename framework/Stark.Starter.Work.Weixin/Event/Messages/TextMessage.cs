namespace Stark.Starter.Work.Weixin.Event.Messages;

/// <summary>
/// 文本消息
/// </summary>
public class TextMessage : MessageBase
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public  const string MsgType = "text";
    
    /// <summary>
    /// 文本消息内容,最长不超过2048个字节，超过将截断
    /// </summary>
    public string Context { get; set; }
}