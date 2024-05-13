using Stark.Module.AI;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Stark.Module.Inf;
using Stark.Module.System;
using Stark.Module.Test;
using Stark.Starter.Core.Extensions;
using Stark.Starter.Web;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Stark.Admin;

[DependsOn(typeof(StarkStarterWeb),
    typeof(StarkSystemModule),
    typeof(StarkInfModule),
    typeof(StarkAIModule),
    typeof(StarkTestModule)
    )]
public class AdminModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 添加健康检查服务
        context.Services.AddHealthChecks();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        // 获取ApplicationBuilder对象
        var app = context.GetApplicationBuilder();

        // 配置健康检查地址
        if (app is WebApplication webApp)
            webApp.MapHealthChecks("/health");
    }
}