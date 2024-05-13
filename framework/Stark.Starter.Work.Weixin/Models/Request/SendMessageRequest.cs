namespace Stark.Starter.Work.Weixin.Models.Request;

/// <summary>
/// <see cref="https://developer.work.weixin.qq.com/document/path/90236"/>
/// </summary>
public class SendMessageRequest
{
    public string touser { get; set; }
    public string toparty { get; set; }
    public string totag { get; set; }
    public string msgtype { get; set; } = "text";
    public int agentid { get; set; }
    public MessageText text { get; set; }
    public int safe { get; set; }
    public int enable_id_trans { get; set; }
    public int enable_duplicate_check { get; set; }
    public int duplicate_check_interval { get; set; }
}

public class MessageText
{
    public string content { get; set; }
}