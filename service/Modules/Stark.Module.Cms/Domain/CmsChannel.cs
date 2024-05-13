using Stark.Starter.Core;
using Stark.Starter.Core.Const;
using Stark.Starter.DDD.Domain;
using Stark.Starter.DDD.Domain.Entities;
using Volo.Abp;

namespace Stark.Module.Cms.Domain;

/// <summary>
/// 栏目表
/// </summary>
public class CmsChannel : AggregateRoot, ISoftDelete, IEntityStatus
{
    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 状态 Y:正常 N:禁用
    /// </summary>
    public string Status { get; set; } = StarkConst.StatusYes;

    /// <summary>
    /// 父级Id
    /// </summary>
    public string ParentId { get; set; } = StarkConst.TreeRoot;

    /// <summary>
    /// 栏目名称
    /// </summary>
    public string ChannelName { get; set; }
}