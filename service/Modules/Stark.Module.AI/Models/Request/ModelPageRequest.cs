using Stark.Module.AI.Models.Enum;
using Stark.Starter.Web.Models;

namespace Stark.Module.AI.Models.Request;

public class ModelPageRequest : PageRequest
{
    /// <summary>
    /// Ai类型
    /// </summary>
    public AiTypeEnum? Type { get; set; }

    /// <summary>
    /// 模型类型
    /// </summary>
    public AiModelTypeEnum? ModelType { get; set; }
}