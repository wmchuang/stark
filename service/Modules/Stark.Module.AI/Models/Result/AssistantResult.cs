using Newtonsoft.Json;

namespace Stark.Module.AI.Models.Result;

public class BotResult
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 提示词
    /// </summary>
    public string Prompting { get; set; }

    /// <summary>
    /// 温度
    /// </summary>
    public decimal Temperature { get; set; }
    
    /// <summary>
    /// 开场白
    /// </summary>
    public string Opening { get; set; }

    /// <summary>
    /// 开场白 数组集合序列化成字符串
    /// </summary>
    public string StartPrologues { get; set; } = "[]";

    /// <summary>
    /// 知识库标识集合 数组集合序列化成字符串
    /// </summary>
    public string WikiIds { get; set; } = "[]";

    public List<string>? StartPrologueList => StartPrologues.IsNullOrWhiteSpace() ? new() : JsonConvert.DeserializeObject<List<string>>(StartPrologues);

    public List<string>? WikiIdList => WikiIds.IsNullOrWhiteSpace() ? new() : JsonConvert.DeserializeObject<List<string>>(WikiIds);
}