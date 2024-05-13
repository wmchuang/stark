using System.ComponentModel.DataAnnotations;

namespace Stark.Module.System.Models.Requests.SysUser;

public class EditUserPasswordRequest
{
    /// <summary>
    /// 用户id
    /// </summary>
    [Required]
    public string UserId { get; set; } = default!;
    
    
    /// <summary>
    /// 新密码
    /// </summary>
    [Required]
    public string NewPassword { get; set; } = string.Empty;

}