using Stark.Starter.Web.Models;

namespace Stark.Module.System.Models.Requests.SysUser;

public class UserPageRequest : PageRequest
{
    /// <summary>
    /// 部门Id
    /// </summary>
    public string? DeptId { get; set; }
    
    /// <summary>
    /// 用户名称
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    public string? RoleId { get; set; }
}