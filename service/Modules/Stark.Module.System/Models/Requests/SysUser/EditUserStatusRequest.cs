using System.ComponentModel.DataAnnotations;

namespace Stark.Module.System.Models.Requests.SysUser;

public class EditUserStatusRequest
{
    /// <summary>
    /// 用户id
    /// </summary>
    [Required]
    public string UserId { get; set; } = default!;

    /// <summary>
    /// 状态
    /// </summary>
    [Required]
    public string Status { get; set; } = default!;
}