using Stark.Module.AI.Models.Enum;
using Stark.Starter.Web.Models;

namespace Stark.Module.AI.Models.Request;

public class WikiDocumentPageRequest : PageRequest
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
    /// 类型 文件、网页
    /// </summary>
    public AiKDocumentType? Type { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public AiKDocumentStatus? Status { get; set; }

    
}