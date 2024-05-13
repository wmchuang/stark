using System.ComponentModel.DataAnnotations;

namespace Stark.Module.System.Models.Requests.SysUser;

/// <summary>
/// 编辑用户角色
/// </summary>
public class EditUserRoleRequest
{
    /// <summary>
    /// 用户id
    /// </summary>
    [Required]
    public string UserId { get; set; } = default!;

    /// <summary>
    /// 角色id
    /// </summary>
    public List<string> RoleIds { get; set; } = new List<string>(0);
}