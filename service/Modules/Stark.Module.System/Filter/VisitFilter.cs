using System.Diagnostics;
using IPTools.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Stark.Module.System.Application.Services;
using Stark.Module.System.Domain;
using Stark.Starter.Core.Const;
using UAParser;
using Volo.Abp.DependencyInjection;

namespace Stark.Module.System.Filter;

/// <summary>
/// 访问日志过滤器
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class VisitFilter : Attribute, IAsyncActionFilter, ITransientDependency
{
    private readonly SysLogVisitService _sysLogVisitService;
    private readonly ILogger<SysLogVisitService> _logger;

    public VisitFilter(SysLogVisitService sysLogVisitService,ILogger<SysLogVisitService> logger)
    {
        _sysLogVisitService = sysLogVisitService;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 计算接口执行时间
        var timeOperation = Stopwatch.StartNew();
        await next();
        timeOperation.Stop();

        // 获取路由表信息
        var routeData = context.RouteData;
        var actionName = routeData.Values["action"];

        // 获取 HttpContext 和 HttpRequest 对象
        var httpContext = context.HttpContext;
        var httpRequest = httpContext.Request;

        // 获取客户端 IPv4 地址
        var remoteIPv4 = httpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
        var ipLocation = GetIpAddress(remoteIPv4);
        
        _logger.LogInformation(httpContext.Connection.LocalIpAddress?.MapToIPv4()?.ToString());
        _logger.LogInformation(httpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString());
        
        
        // 客户端浏览器信息
        var userAgent = httpRequest.Headers["User-Agent"];
        var client = Parser.GetDefault().Parse(userAgent.ToString());
        var browser = $"{client.UA.Family} {client.UA.Major}.{client.UA.Minor} / {client.Device.Family}";
        var os = $"{client.OS.Family} {client.OS.Major} {client.OS.Minor}";

        // 获取授权用户
        var user = httpContext.User;

        // 获取当前操作者
        string userName = "", loginName = "";
        foreach (var item in user.Claims)
        {
            switch (item.Type)
            {
                case ClaimConst.UserName:
                    userName = item.Value;
                    break;
                case ClaimConst.LoginName:
                    loginName = item.Value;
                    break;
            }
        }

        var sysLogVisit = new SysLogVisit
        {
            ActionName = actionName?.ToString(),
            RemoteIp = remoteIPv4,
            Location = ipLocation,
            Os = os,
            Browser = browser,
            Elapsed = timeOperation.ElapsedMilliseconds,
            UserName = userName,
            LoginName = loginName
        };

        await _sysLogVisitService.AddAsync(sysLogVisit);
    }

    /// <summary>
    /// 解析IP地址
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    private static string GetIpAddress(string? ip)
    {
        if (!string.IsNullOrWhiteSpace(ip))
        {
            try
            {
                var ipInfo = IpTool.Search(ip);
                var addressList = new List<string>()
                    { ipInfo.Country, ipInfo.Province, ipInfo.City, ipInfo.NetworkOperator };
                return string.Join("|", addressList.Where(it => it != "0").ToList());
            }
            catch
            {
                // ignored
            }
        }

        return "未知";
    }
}