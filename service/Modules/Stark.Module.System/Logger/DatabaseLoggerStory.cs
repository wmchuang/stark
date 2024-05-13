using Microsoft.Extensions.DependencyInjection;
using Stark.Module.System.Application.Services;
using Stark.Module.System.Domain;
using Stark.Starter.Web.Logger;

namespace Stark.Module.System.Logger;

/// <summary>
/// 
/// </summary>
public class DatabaseLoggerStory : IDatabaseLoggerStory
{
    private readonly SysLogExService _sysLogExService;

    public DatabaseLoggerStory(IServiceScopeFactory scopeFactory)
    {
        var serviceScope = scopeFactory.CreateScope();
        _sysLogExService = serviceScope.ServiceProvider.GetRequiredService<SysLogExService>();
        ;
    }

    /// <summary>
    /// 保存异常日志
    /// </summary>
    /// <param name="logInfo"></param>
    public async void SaveAsync(LogInfo logInfo)
    {
        var sysLogEx = new SysLogEx()
        {
            Message = logInfo.Message,
            StackTrace = logInfo.Exception.StackTrace,
            RequestUrl = logInfo.RequestUrl,
            Source = logInfo.Exception.Source,
            LoginName = logInfo.LoginName,
            UserName = logInfo.UserName,
        };

        await _sysLogExService.AddAsync(sysLogEx);
    }
}