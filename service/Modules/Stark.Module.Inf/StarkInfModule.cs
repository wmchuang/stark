using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Stark.Module.Inf.Options;
using Stark.Module.Inf.Services;
using Stark.Starter.Cap;
using Stark.Starter.Web;
using Volo.Abp.Modularity;

namespace Stark.Module.Inf;

/// <summary>
/// 基础服务模块 包括文件管理等
/// </summary>
[DependsOn(
    typeof(StarkStarterWeb),
    typeof(StarkStarterCap),
    typeof(StarkStarterCap)
    )]
public class StarkInfModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // // 添加Oss配置文件
        // var ossSetting = ConfigurationHelper.BuildConfiguration(new AbpConfigurationBuilderOptions
        // {
        //     BasePath = AppContext.BaseDirectory,
        //     // 这里不需要添加 .json后缀
        //     FileName = "fileSetting"
        // });
        //
        // // 添加配置
        // context.Services.Configure<FileSetting>(ossSetting.GetSection("FileSetting"));
        
        // 获取配置
        var configuration = context.Services.GetConfiguration();
        var setting = configuration.GetSection("FileSetting").Get<FileSetting>();
        if (setting?.IsUseOss ?? false)
        {
            context.Services.AddTransient<IFileService, OssService>();
        }
        else
        {
            context.Services.AddTransient<IFileService, LocalService>();
        }
        
        Console.WriteLine("基础服务模块配置完成");
    }
}