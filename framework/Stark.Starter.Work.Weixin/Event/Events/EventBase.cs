namespace Stark.Starter.Work.Weixin.Event.Events;

/// <summary>
/// 基础信息结构
/// </summary>
public abstract class EventBase
{
    /// <summary>
    /// 企业微信CorpID
    /// </summary>
    public string ToUserName { get; set; }

    /// <summary>
    /// 此事件该值固定为sys，表示该消息由系统生成
    /// </summary>
    public string FromUserName { get; set; }

    /// <summary>
    /// 消息创建时间
    /// </summary>
    public long CreateTime { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public static string MsgType => "event";
}