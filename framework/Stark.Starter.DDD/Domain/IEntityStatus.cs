namespace Stark.Starter.DDD.Domain;

/// <summary>
/// 实体可用状态
/// </summary>
public interface IEntityStatus
{
    /// <summary>
    /// 状态 Y:可用 N:禁用
    /// </summary>
    public string Status { get; set; }
}