using Stark.Starter.Web.Models;

namespace Stark.Module.System.Models.Requests.SysRoles;

public class RolePageRequest : PageRequest
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public string RoleName { get; set; } = string.Empty;
}