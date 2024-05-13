namespace Stark.Module.AI.Models.Request;

public class GetSourceRelevantRequest
{
    /// <summary>
    /// 知识库标识
    /// </summary>
    public string WikiId { get; set; }

    /// <summary>
    /// 用户输入的文本
    /// </summary>
    public string Context { get; set; }
}