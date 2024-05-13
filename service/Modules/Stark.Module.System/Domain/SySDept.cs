using Stark.Starter.Core;
using Stark.Starter.Core.Const;
using Stark.Starter.DDD.Domain;
using Stark.Starter.DDD.Domain.Entities;
using Volo.Abp;

namespace Stark.Module.System.Domain;

/// <summary>
/// 部门
/// </summary>
public class SySDept : AggregateRoot, ISoftDelete, IEntityStatus
{
    public SySDept()
    {
        TreePath = string.Empty;
        ParentDeptId = string.Empty;
    }

    public SySDept(string deptName, int sort) : this()
    {
        DeptName = deptName;
        Sort = sort;
    }

    /// <summary>
    /// 部门名称
    /// </summary>
    public string DeptName { get; set; }

    /// <summary>
    /// 父级部门Id
    /// </summary>
    public string ParentDeptId { get; set; }

    /// <summary>
    /// 节点路径
    /// </summary>
    public string TreePath { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 状态 Y:正常 N:禁用
    /// </summary>
    public string Status { get; set; } = StarkConst.StatusYes;

    /// <summary>
    /// 设置路径
    /// </summary>
    /// <param name="parentDept"></param>
    public void SetTreePath(SySDept parentDept)
    {
        ParentDeptId = parentDept.Id;
        TreePath = parentDept.TreePath + "," + parentDept.Id;
        TreePath = TreePath.Trim(',');
    }
    
    public void SetEmptyParent()
    {
        ParentDeptId = StarkConst.TreeRoot;
        TreePath = string.Empty;
    }
    
    public int GetLevel()
    {
        return TreePath.Split(',').Count();
    }
}