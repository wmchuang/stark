using System.Collections.ObjectModel;
using SqlSugar;
using Stark.Starter.DDD.Domain.DomainEvent;

namespace Stark.Starter.DDD.Domain.Entities;

/// <summary>
/// 聚合根
/// </summary>
public class AggregateRoot : Entity
{
    public AggregateRoot()
    {
        CreateBy = string.Empty;
        CreateName = string.Empty;
        UpdateBy = string.Empty;
        UpdateName = string.Empty;
    }

    List<ILocalDomainEvent>? _localDomainEvents;

    /// <summary>
    /// 获取本地领域事件
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public IReadOnlyCollection<ILocalDomainEvent> LocalDomainEvents =>
        _localDomainEvents?.AsReadOnly() ?? new ReadOnlyCollection<ILocalDomainEvent>(new List<ILocalDomainEvent>(0));

    protected void AddLocalEvent(ILocalDomainEvent eventItem)
    {
        _localDomainEvents = _localDomainEvents ?? new List<ILocalDomainEvent>();
        _localDomainEvents.Add(eventItem);
    }

    public void ClearLocalEvent(ILocalDomainEvent eventItem)
    {
        _localDomainEvents?.Remove(eventItem);
    }

    public void ClearLocalEvents()
    {
        _localDomainEvents?.Clear();
    }

    // public virtual string ConcurrencyStamp { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string CreateBy { get; set; }

    /// <summary>
    /// 创建人名称
    /// </summary>
    public string CreateName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string UpdateBy { get; set; }

    /// <summary>
    /// 修改人名称
    /// </summary>
    public string UpdateName { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    #region 实体操作

    /// <summary>
    /// 添加创建人信息
    /// </summary>
    /// <param name="operatorId"></param>
    /// <param name="operatorName"></param>
    public virtual void SetCreate(string operatorId, string operatorName)
    {
        CreateBy = operatorId;
        CreateName = operatorName;
        CreateTime = DateTime.Now;
        SetModifier(operatorId, operatorName);
    }

    /// <summary>
    /// 添加修改人信息
    /// </summary>
    /// <param name="operatorId"></param>
    /// <param name="operatorName"></param>
    public virtual void SetModifier(string operatorId, string operatorName)
    {
        UpdateBy = operatorId;
        UpdateName = operatorName;
        UpdateTime = DateTime.Now;
    }

    #endregion
}