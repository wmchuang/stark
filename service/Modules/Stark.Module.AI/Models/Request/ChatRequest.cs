namespace Stark.Module.AI.Models.Request;

public class ChatRequest
{
    /// <summary>
    /// 智能体标识
    /// </summary>
    public string BotId { get; set; }
    
    /// <summary>
    /// 知识库标识
    /// </summary>
    public string? WikiId { get; set; }
    
    /// <summary>
    /// 会话标识
    /// </summary>
    public string ChatId { get; set; }
    
    /// <summary>
    /// 消息
    /// </summary>
    public string Context { get; set; }

    /// <summary>
    /// 流式输出
    /// </summary>
    public bool Stream { get; set; } = true;
}