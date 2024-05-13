
using Stark.Starter.DDD.Domain.Entities;

namespace Stark.Module.System.Domain;

public class SysUserRole : Entity
{
    #region 构造函数

    public SysUserRole()
    {
    }

    public SysUserRole(string userId, string roleId) : this()
    {
        UserId = userId;
        RoleId = roleId;
    }

    #endregion

    #region 实体类

    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; private set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    public string RoleId { get; private set; }

    #endregion
}