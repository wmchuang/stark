namespace Stark.Module.AI.Models.Dto;

/// <summary>
/// 文档来源相关度
/// </summary>
public class SourceRelevant
{
    /// <summary>
    /// 
    /// </summary>
    public string SourceName { get; set; }

    public string? SourceUrl { get; set; }
    public string Text { get; set; }
    public float Relevance { get; set; }

    public DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.MinValue;

    public override string ToString()
    {
        return $"==== [文件:{SourceName};相关性:{(Relevance * 100):F2}%]:{Text}{Environment.NewLine}";
    }
}