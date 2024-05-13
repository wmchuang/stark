using Stark.Starter.DDD.Domain;
using Stark.Starter.DDD.Domain.Entities;
using Stark.Starter.Core.Const;
using Volo.Abp;

namespace Stark.Module.Cms.Domain;

/// <summary>
/// Banner
/// </summary>
public class CmsBanner : AggregateRoot, ISoftDelete, IEntityStatus
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
    /// 图片
    /// </summary>
    public string Pic { get; set; } = string.Empty;

    /// <summary>
    /// 连接
    /// </summary>
    public string Link { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}