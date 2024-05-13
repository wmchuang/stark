using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stark.Module.System.Application.Services;
using Stark.Module.System.Filter;
using Stark.Module.System.Models.Requests.Auth;
using Stark.Module.System.Models.Results.Auth;
using Stark.Starter.Web.Filters;
using Stark.Starter.Web.Jwt;
using Volo.Abp;

namespace Stark.Module.System.Controllers;

/// <summary>
/// 授权
/// </summary>
public class AuthController : BaseController
{
    private readonly ICaptcha _captcha;
    private readonly AuthService _authService;

    public AuthController(ICaptcha captcha, AuthService authService)
    {
        _captcha = captcha;
        _authService = authService;
    }

    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<GetCaptchaResult> Captcha()
    {
        var key = Guid.NewGuid().ToString("N");
        var info = await Task.Run(() => _captcha.Generate(key));
        return new GetCaptchaResult
        {
            CaptchaKey = key,
            CaptchaBase64 = info.Base64
        };
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    [HttpPost]
    [AllowAnonymous]
    public async Task<string> Login(LoginRequest request)
    {
        var valid = _captcha.Validate(request.CaptchaKey, request.CaptchaCode);
        if (!valid)
            throw new UserFriendlyException("验证码错误");

        return await _authService.LoginAsync(request);
    }

    /// <summary>
    /// 获取路由权限
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilter(typeof(VisitFilter))]
    public async Task<UserRouterResult> GetRouteAsync()
    {
        return await _authService.GetRouteAsync();
    }
}