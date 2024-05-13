namespace Stark.Module.System.Models.Requests.Auth;

/// <summary>
/// 登录请求参数
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// 验证码key
    /// </summary>
    public string CaptchaKey { get; set; }
    
    /// <summary>
    /// 验证码
    /// </summary>
    public string CaptchaCode { get; set; }
}