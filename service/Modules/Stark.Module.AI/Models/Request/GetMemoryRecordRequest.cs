namespace Stark.Module.AI.Models.Request;

public class GetMemoryRecordRequest
{
    /// <summary>
    /// 知识库标识
    /// </summary>
    public string WikiId { get; set; }

    /// <summary>
    /// 文档Id
    /// </summary>
    public string? DocumentId { get; set; }
}