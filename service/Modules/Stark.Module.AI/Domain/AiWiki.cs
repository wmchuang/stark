using Stark.Module.AI.Models.Enum;
using Stark.Starter.DDD.Domain.Entities;

namespace Stark.Module.AI.Domain;

/// <summary>
/// 知识库
/// </summary>
public class AiWiki : AggregateRoot
{
    /// <summary>
    /// 知识库名称
    /// </summary>
    public string WikiName { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 会话模型ID
    /// </summary>
    public string ChatModelId { get; set; }

    /// <summary>
    /// 向量模型ID
    /// </summary>
    public string EmbeddingModelId { get; set; }

    /// <summary>
    /// 保存位置
    /// </summary>
    public WikiSaveDbType DbType { get; set; }

    /// <summary>
    /// 数据库连接地址
    /// </summary>
    public string? ConnectionString { get; set; }
}