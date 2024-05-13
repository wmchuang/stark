using Stark.Module.AI.Models.Enum;
using Stark.Starter.DDD.Domain.Entities;

namespace Stark.Module.AI.Domain;

/// <summary>
/// 大模型
/// </summary>
public class AiModel : AggregateRoot
{
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Ai类型
    /// </summary>
    public AiTypeEnum Type { get; set; } = AiTypeEnum.OpenAi;

    /// <summary>
    /// 模型类型
    /// </summary>
    public AiModelTypeEnum ModelType { get; set; } = AiModelTypeEnum.Chat;

    /// <summary>
    /// 模型地址
    /// </summary>
    public string EndPoint { get; set; } = "";

    /// <summary>
    /// 模型名称
    /// </summary>
    public string ModelName { get; set; } = "";

    /// <summary>
    /// 模型秘钥
    /// </summary>
    public string ModelKey { get; set; } = "";
}