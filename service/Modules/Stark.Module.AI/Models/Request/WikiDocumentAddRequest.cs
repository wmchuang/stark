using Stark.Module.AI.Models.Enum;

namespace Stark.Module.AI.Models.Request;

public class WikiDocumentAddRequest
{
    /// <summary>
    /// 知识库标识
    /// </summary>
    public string WikiId { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; } = "";

    /// <summary>
    /// 文件地址 或网页url
    /// </summary>
    public string Path { get; set; } = "";

    /// <summary>
    /// 文本
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// 类型 0:文件、 1:网页 2:文本
    /// </summary>
    public AiKDocumentType Type { get; set; }
}