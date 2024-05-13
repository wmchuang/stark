using System.ComponentModel;

namespace Stark.Module.AI.Models.Enum;


public enum AiTypeEnum
{
    /// <summary>
    /// OpenAi
    /// </summary>
    [Description("OpenAi")]
    OpenAi = 0,
    
    /// <summary>
    /// AzureOpenAi
    /// </summary>
    [Description("AzureOpenAi")]
    AzureOpenAi,
}