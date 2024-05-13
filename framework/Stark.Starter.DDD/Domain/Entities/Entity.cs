using System.ComponentModel.DataAnnotations;
using SqlSugar;
using Yitter.IdGenerator;

namespace Stark.Starter.DDD.Domain.Entities;

/// <summary>
/// 实体
/// </summary>
public class Entity
{
    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(IsPrimaryKey = true)]
    [Key]
    public virtual string Id { get; protected set; } = YitIdHelper.NextId().ToString();
}