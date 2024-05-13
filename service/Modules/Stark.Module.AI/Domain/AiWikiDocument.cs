using Stark.Module.AI.Models.Enum;
using Stark.Starter.DDD.Domain.Entities;

namespace Stark.Module.AI.Domain;

/// <summary>
/// 知识库文档
/// </summary>
public class AiWikiDocument : Entity
{
    public AiWikiDocument()
    {
        CreateTime = DateTime.Now;
    }

    /// <summary>
    /// 知识库标识
    /// </summary>
    public string WikiId { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; } = "";

    /// <summary>
    /// 地址 (文件地址 或者 网页url)
    /// </summary>
    public string Path { get; set; } = "";

    /// <summary>
    /// 文本
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// 类型 文件、网页
    /// </summary>
    public AiKDocumentType Type { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public AiKDocumentStatus Status { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}