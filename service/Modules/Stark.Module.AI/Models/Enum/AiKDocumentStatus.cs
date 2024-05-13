using System.ComponentModel;

namespace Stark.Module.AI.Models.Enum;

public enum AiKDocumentStatus
{
    /// <summary>
    /// 处理中
    /// </summary>
    [Description("处理中")]
    Logging = 0,

    /// <summary>
    /// 成功
    /// </summary>
    [Description("成功")]
    Success,

    /// <summary>
    /// 失败
    /// </summary>
    [Description("失败")]
    Fail
}