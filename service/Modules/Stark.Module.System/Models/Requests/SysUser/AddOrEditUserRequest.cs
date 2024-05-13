namespace Stark.Module.System.Models.Requests.SysUser;

public class AddOrEditUserRequest
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///  部门ID
    /// </summary>
    public string DeptId { get; set; }

    /// <summary>
    /// 登录账号
    /// </summary>
    public string LoginName { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// 状态 Y:正常 N:禁用
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}