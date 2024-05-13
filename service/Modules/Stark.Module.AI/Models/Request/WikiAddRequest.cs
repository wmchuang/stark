using Stark.Module.AI.Models.Enum;

namespace Stark.Module.AI.Models.Request;

/// <summary>
/// 知识库添加
/// </summary>
public class WikiAddRequest
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

/// <summary>
/// 知识库修改
/// </summary>
public class WikiUpdateRequest : WikiAddRequest
{

    /// <summary>
    /// 知识库标识
    /// </summary>
    public string Id { get; set; }

}
