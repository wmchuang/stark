using System.ComponentModel;

namespace Stark.Module.AI.Models.Enum;


public enum AiModelTypeEnum
{
    /// <summary>
    /// 会话模型
    /// </summary>
    [Description("会话模型")]
    Chat = 0,
    
    /// <summary>
    /// 嵌入模型
    /// </summary>
    [Description("嵌入模型")]
    Embedding,
}