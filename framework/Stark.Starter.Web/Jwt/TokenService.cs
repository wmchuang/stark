using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stark.Starter.Core.Const;

namespace Stark.Starter.Web.Jwt;

/// <summary>
/// Token服务
/// </summary>
public class TokenService
{
    private readonly JwtSettingsOptions _jwtKeyConfig;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="jwtKeyConfig"></param>
    public TokenService(IOptions<JwtSettingsOptions> jwtKeyConfig)
    {
        _jwtKeyConfig = jwtKeyConfig.Value;
    }

    /// <summary>
    /// 创建Token
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userName"></param>
    /// <param name="loginName"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    public Task<string> CreateTokenAsync(string userId,string userName,string loginName, Claim[]? payload = default)
    {
        var claims = new[]
        {
            new Claim(ClaimConst.UserId, userId),
            new Claim(ClaimConst.UserName, userName),
            new Claim(ClaimConst.LoginName, loginName),
        };
        if (payload?.Length > 0)
        {
            claims = claims.Concat(payload).ToArray();
        }

        var token = CreateToken(claims);
        return Task.FromResult(token);
    }

    /// <summary>
    /// 生成Token
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    private string CreateToken(Claim[] claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKeyConfig.IssuerSigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtKeyConfig.ValidIssuer,
            _jwtKeyConfig.ValidAudience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtKeyConfig.ExpiredTime ?? 10080),
            signingCredentials: credentials);

        return $"Bearer {new JwtSecurityTokenHandler().WriteToken(token)}";
    }
}