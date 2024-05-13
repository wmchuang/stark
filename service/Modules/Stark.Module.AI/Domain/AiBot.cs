using Stark.Starter.DDD.Domain.Entities;

namespace Stark.Module.AI.Domain;

/// <summary>
/// 智能体
/// </summary>
public class AiBot : AggregateRoot
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
    /// 推荐问题集合 数组集合序列化成字符串
    /// </summary>
    public string StartPrologues { get; set; } = "[]";
        
    /// <summary>
    /// 知识库标识集合 数组集合序列化成字符串
    /// </summary>
    public string WikiIds { get; set; } = "[]";
    

}