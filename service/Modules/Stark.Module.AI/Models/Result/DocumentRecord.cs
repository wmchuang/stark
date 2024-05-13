namespace Stark.Module.AI.Models.Result;

public class DocumentRecord
{
    /// <summary>
    /// 文档标识
    /// </summary>
    public string DocumentId { get; set; }

    /// <summary>
    /// 文本
    /// </summary>
    public string Text { get; set; }

    public string? WebPageUrl { get; set; }

    public DateTimeOffset LastUpdate { get; set; }

    public string File { get; set; }
}