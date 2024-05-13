namespace Stark.Module.System.Models.Requests.SysUser;

public class EditUserInfoRequest
{
    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; set; } = default!;

    /// <summary>
    /// 手机号
    /// </summary>
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; } = string.Empty;
}