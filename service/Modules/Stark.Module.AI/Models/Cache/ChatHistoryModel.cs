using Microsoft.SemanticKernel.ChatCompletion;

namespace Stark.Module.AI.Models.Cache;

public class ChatHistoryModel
{
    /// <summary>
    /// 角色
    /// </summary>
    public AuthorRole Role { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Context { get; set; }
}