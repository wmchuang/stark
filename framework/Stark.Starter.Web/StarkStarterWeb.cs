using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Stark.Starter.Core.Extensions;
using Stark.Starter.Web.Filters;
using Stark.Starter.Web.Format;
using Stark.Starter.Web.Hub;
using Stark.Starter.Web.Interceptors;
using Stark.Starter.Web.Swagger;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Yitter.IdGenerator;

namespace Stark.Starter.Web;

[DependsOn(
    typeof(AbpAutofacModule)
)]
public class StarkStarterWeb : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        base.PreConfigureServices(context);
        //注册拦截器（AOP）对IOC的类进行代理
        context.Services.OnRegistered(options =>
        {
            if (options.ImplementationType.IsDefined(typeof(LogAttribute), true))
            {
                options.Interceptors.TryAdd<LogInterceptor>();
            }
        });
    }

    /// <summary>
    /// 这是配置模块和注册服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 添加ObjectAccessor，用于访问IApplicationBuilder对象
        context.Services.AddObjectAccessor<IApplicationBuilder>();

        // 添加日志服务
        context.Services.AddLogging();

        // 配置Swagger
        context.Services.AddSwaggerConfig(option => { });

        // 添加JWT服务
        context.Services.AddJwt();

        // 配置跨域服务
        context.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        // 设置Id生成器
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions(1));

        // 添加控制器服务
        context.Services.AddControllers(options =>
        {
            options.Filters.Add<DataValidationFilter>();
            options.Filters.Add<ResultFilter>();
        }).ConfigureApiBehaviorOptions(options =>
        {
            // 关闭模型验证
            options.SuppressModelStateInvalidFilter = true;
        }).AddJsonOptions(options =>
        {
            //自定义输出的时间格式 System.Text.Json的配置
            options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
        });

        // 添加HttpContext访问器
        context.Services.AddHttpContextAccessor();

        // 使用转发头中间件
        context.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        context.Services.AddSignalR();
            
        // 调用基类的ConfigureServices方法
        base.ConfigureServices(context);
    }

    /// <summary>
    /// 这里用来配置请求管道并初始化服务
    /// </summary>
    /// <param name="context"></param>
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        // 获取ApplicationBuilder对象
        var app = context.GetApplicationBuilder();

        // 使用转发头中间件
        app.UseForwardedHeaders();
        
        // 使用路由中间件
        app.UseRouting();

        // 使用允许所有请求的中间件
        app.UseCors("AllowAll");

        // 使用身份验证和授权中间件
        app.UseAuthentication();
        app.UseAuthorization();

        // 使用Swagger UI中间件
        app.UserSwaggerUi();

        app.UseStaticFiles();

        // 配置结束点，映射控制器
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        
        app.UseEndpoints(endpoints => { endpoints.MapHub<NoticeHub>("/api/hubs/noticeHub"); });
    }
}