
using System.ComponentModel;

namespace Stark.Module.AI.Models.Enum;

public enum AiKDocumentType
{
    /// <summary>
    /// 文件
    /// </summary>
    [Description("文件")]
    File = 0,

    /// <summary>
    /// 网页
    /// </summary>
    [Description("网页")]
    WebPage,
    
    /// <summary>
    /// 文本
    /// </summary>
    [Description("文本")]
    Text,
}