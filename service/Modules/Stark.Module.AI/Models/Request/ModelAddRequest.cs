using Stark.Module.AI.Models.Enum;

namespace Stark.Module.AI.Models.Request;

public class ModelAddRequest
{
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Ai类型 0: OpenAi 1：AzureOpenAi
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

public class ModelUpdateRequest : ModelAddRequest
{
    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; }
}