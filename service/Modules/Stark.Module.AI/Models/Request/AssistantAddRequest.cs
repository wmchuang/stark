namespace Stark.Module.AI.Models.Request;

public class BotAddRequest
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
    /// 会话模型ID
    /// </summary>
    public string ChatModelId { get; set; }

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
    /// 推荐问题 数组集合
    /// </summary>
    public List<string> StartPrologues { get; set; }

    /// <summary>
    /// 知识库标识集合
    /// </summary>
    public List<string> WikiIds { get; set; }
}

public class BotUpdateRequest : BotAddRequest
{
    public string Id { get; set; }
}