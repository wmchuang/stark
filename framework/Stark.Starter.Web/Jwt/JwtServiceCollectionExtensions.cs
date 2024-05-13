using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Stark.Starter.Web.Jwt;

namespace Microsoft.Extensions.DependencyInjection;

public static class JwtServiceCollectionExtensions
{
    public static void AddJwt(this IServiceCollection services)
    {
        services.AddTransient<TokenService>();
        
        // 添加默认授权
        var authenticationBuilder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        //获取配置
        var jwtSettings = new JwtSettingsOptions();
        services.AddOptions<JwtSettingsOptions>()
            .BindConfiguration("JWTSettings")
            .Configure(options => { jwtSettings = options; });

        authenticationBuilder.AddJwtBearer(options =>
        {
            // 配置 JWT 验证信息
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // 验证签发方密钥
                ValidateIssuerSigningKey = true,
                // 签发方密钥
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey)),
                // 验证签发方
                ValidateIssuer = true,
                // 设置签发方
                ValidIssuer = jwtSettings.ValidIssuer,
                // 验证签收方
                ValidateAudience = true,
                // 设置接收方
                ValidAudience = jwtSettings.ValidAudience,
                // 验证生存期
                ValidateLifetime = true,
                // 过期时间容错值
                ClockSkew = TimeSpan.FromSeconds(jwtSettings?.ClockSkew ?? 10),
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = (context) =>
                {
                    if (!context.HttpContext.Request.Path.HasValue)
                    {
                        return Task.CompletedTask;
                    }
                    var accessToken = context.HttpContext.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!accessToken.IsNullOrEmpty() && path.StartsWithSegments("/api/hubs"))
                    {
                        context.Token = accessToken;
                        return Task.CompletedTask;
                    }
                    return Task.CompletedTask;
                }
            };
        });
    }
}