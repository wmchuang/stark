using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stark.Module.AI.Backgrounds;
using Stark.Module.AI.Infrastructure;
using Stark.Module.AI.Options;
using Stark.Starter.DDD;
using Stark.Starter.DDD.Infrastructure.EFCore;
using Stark.Starter.Redis;
using Stark.Starter.Web;
using Stark.Starter.Work.Weixin;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Stark.Module.AI;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(StarkStarterDDD),
    typeof(StarkStarterWeb),
    typeof(StarkStarterRedis),
    typeof(StarkStarterWorkWeiXin)
)]

public class StarkAIModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 添加Ai配置文件
        // var aiSetting = ConfigurationHelper.BuildConfiguration(new AbpConfigurationBuilderOptions
        // {
        //     BasePath = AppContext.BaseDirectory,
        //     // 这里不需要添加 .json后缀
        //     FileName = "aiSetting"
        // });


        
        // 获取配置
        var configuration = context.Services.GetConfiguration();
        configuration.GetSection("WorkAiOption").Get<WorkAiOption>();
        
        // 获取数据库连接字符串
        var connectionString = configuration.GetConnectionString("ConnectionConfigs");
        // 获取数据库类型
        var dbType = configuration.GetConnectionString("DbType");
        // 添加数据库上下文
        context.Services.AddDbContext<AiDbContext>(options =>
        {
            options.UseDb(connectionString,dbType);
            //开发环境可以打开日志记录和显示详细的错误
            if (context.Services.GetAbpHostEnvironment().IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        context.Services.AddHostedService<QuantizeBackgroundService>();
        context.Services.AddHostedService<WorkWxBackgroundService>();
        
        Console.WriteLine("AI模块配置完成");
    }
    
    /// <summary>
    /// 初始化所有模块之后
    /// </summary>
    /// <param name="context"></param>
    public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
    {
// #if DEBUG
        //DEBUG 模式下 自动迁移
        var dbContext = context.ServiceProvider.GetRequiredService<AiDbContext>();
        dbContext.Database.MigrateAsync();
// #endif
    }
}