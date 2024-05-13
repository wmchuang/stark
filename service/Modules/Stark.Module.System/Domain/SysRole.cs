using Stark.Starter.Core.Enum;
using Stark.Starter.DDD.Domain;
using Stark.Starter.DDD.Domain.Entities;
using Volo.Abp;

namespace Stark.Module.System.Domain;

public class SysRole : AggregateRoot, IEntityStatus, ISoftDelete
{
    #region 构造函数

    public SysRole()
    {
        Remark = string.Empty;
    }

    public SysRole(string name, string key, int sortNumber, RoleDataScopeEnum dataScope, string remark) : this()
    {
        Name = name;
        Key = key;
        Sort = sortNumber;
        DataScope = dataScope;
        Remark = remark;
    }

    #endregion

    #region 实体类

    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 角色权限
    /// </summary>
    public string Key { get; private set; }

    /// <summary>
    /// 角色排序
    /// </summary>
    public int Sort { get; private set; }

    /// <summary>
    ///数据范围
    /// </summary>
    public RoleDataScopeEnum DataScope { get; private set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; private set; }

    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; }

    #endregion

    #region 操作

    public SysRole SetBaseInfo(string name, string key, int sortNumber, string remark)
    {
        Name = name;
        Key = key;
        Sort = sortNumber;
        Remark = remark;
        return this;
    }

    public SysRole SetDataScope(RoleDataScopeEnum dataScope)
    {
        DataScope = dataScope;
        return this;
    }

    #endregion
}