using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stark.Starter.Work.Weixin.Models;
using Volo.Abp.Modularity;

namespace Stark.Starter.Work.Weixin;

/// <summary>
/// 企业微信启动模块
/// </summary>
public class StarkStarterWorkWeiXin : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var section = context.Services.GetConfiguration().GetSection("WorkWxConfig");

        var exist = section.Exists();
        if (!exist)
        {
            throw new Exception("请在appsettings.json中配置WorkWxConfig");
        }

        context.Services.AddHttpClient();
        context.Services.Configure<WorkWxConfig>(section);
    }
}