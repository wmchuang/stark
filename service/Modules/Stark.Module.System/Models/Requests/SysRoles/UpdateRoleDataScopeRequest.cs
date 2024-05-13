using System.ComponentModel.DataAnnotations;
using Stark.Starter.Core.Enum;

namespace Stark.Module.System.Models.Requests.SysRoles;

/// <summary>
/// 修改角色数据范围请求
/// </summary>
public class UpdateRoleDataScopeRequest
{
    /// <summary>
    /// 角色id
    /// </summary>
    [Required(ErrorMessage = "角色id不能为空")]
    public string RoleId { get; set; }

    /// <summary>
    /// 数据范围
    /// </summary>
    public RoleDataScopeEnum DataScope { get; set; }
}