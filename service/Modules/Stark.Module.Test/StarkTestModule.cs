using Microsoft.Extensions.DependencyInjection;
using Stark.Module.Test.Options;
using Stark.Starter.Cap;
using Stark.Starter.Job;
using Volo.Abp.Modularity;

namespace Stark.Module.Test;

[DependsOn(
    typeof(StarkStarterCap),
    typeof(StarkStarterJob)
)]
public class StarkTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 获取配置
        var configuration = context.Services.GetConfiguration();
        
        // 添加配置
        context.Services.Configure<JobConfig>(configuration.GetSection(JobConfig.Position));
    }
}