using System.ComponentModel.DataAnnotations;

namespace Stark.Module.System.Models.Requests.SysUser;

public class EditMyPasswordRequest
{
    /// <summary>
    /// 旧密码
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// 新密码
    /// </summary>
    [Required]
    public string NewPassword { get; set; } = string.Empty;
}