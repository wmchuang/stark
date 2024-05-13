using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using Stark.Module.System.Infrastructure;
using Stark.Module.System.Logger;
using Stark.Starter.DDD;
using Stark.Starter.DDD.Infrastructure.EFCore;
using Stark.Starter.Web;
using Stark.Starter.Web.Logger;
using Stark.Module.System.Application.Mapper;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Stark.Module.System;

/// <summary>
/// 系统管理模块 包含常用RBAC权限管理功能
/// </summary>
[DependsOn(
    typeof(StarkStarterDDD),
    typeof(StarkStarterWeb))]
public class StarkSystemModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        // 获取配置
        var configuration = context.Services.GetConfiguration();

        // 获取数据库连接字符串
        var connectionString = configuration.GetConnectionString("ConnectionConfigs");

        // 获取数据库类型
        var dbType = configuration.GetConnectionString("DbType");
        // 添加数据库上下文
        context.Services.AddDbContext<SystemDbContext>(options =>
        {
            options.UseDb(connectionString, dbType);

            //开发环境可以打开日志记录和显示详细的错误
            if (context.Services.GetAbpHostEnvironment().IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // 添加验证码服务
        var captchaConfig = ConfigurationHelper.BuildConfiguration(new AbpConfigurationBuilderOptions
        {
            BasePath = AppContext.BaseDirectory,
            // 这里不需要添加 .json后缀
            FileName = "captcha"
        });
        context.Services.AddCaptcha(captchaConfig);

        // 添加 AutoMapper
        // context.Services.AddAutoMapper(typeof(AutoMapperProfile));

        // 添加映射服务
        context.Services.AddMapste();

        // 添加MediatR服务，
        context.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(StarkSystemModule).Assembly));

        // 添加日志服务
        context.Services.AddLogging(x => x.AddDatabaseLogger<DatabaseLoggerStory>());

        Console.WriteLine("系统管理模块配置完成");
    }

    /// <summary>
    /// 初始化所有模块之后
    /// </summary>
    /// <param name="context"></param>
    public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
    {
//#if DEBUG
        //DEBUG 模式下 自动迁移
        var dbContext = context.ServiceProvider.GetRequiredService<SystemDbContext>();
        dbContext.Database.MigrateAsync();
//#endif
    }
}