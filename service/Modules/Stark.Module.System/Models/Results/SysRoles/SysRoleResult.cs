using Stark.Starter.Core.Enum;
using Stark.Starter.Core.Extensions;

namespace Stark.Module.System.Models.Results.SysRoles;

public class SysRoleResult
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 角色权限
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// 角色排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 数据范围
    /// </summary>
    public RoleDataScopeEnum DataScope { get; set; }

    /// <summary>
    /// 数据范围名称
    /// </summary>
    public string DataScopeName => DataScope.GetDescriptionString();

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; } = string.Empty;
}