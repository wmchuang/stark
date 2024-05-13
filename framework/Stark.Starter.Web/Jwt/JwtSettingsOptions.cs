namespace Stark.Starter.Web.Jwt;

/// <summary>
/// Jwt 配置
/// </summary>
public sealed class JwtSettingsOptions
{
    /// <summary>
    /// 签发方密钥
    /// </summary>
    public string IssuerSigningKey { get; set; } = "IssuerSigningKey";

    /// <summary>
    /// 签发方
    /// </summary>
    public string ValidIssuer { get; set; } = "ValidIssuer";

    /// <summary>
    /// 签收方
    /// </summary>
    public string ValidAudience { get; set; }  = "ValidAudience";

    /// <summary>
    /// 过期时间容错值，解决服务器端时间不同步问题（秒）
    /// </summary>
    public long? ClockSkew { get; set; }

    /// <summary>
    /// 过期时间（分钟）
    /// </summary>
    public long? ExpiredTime { get; set; }
}