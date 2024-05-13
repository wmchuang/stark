using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SqlSugar;
using Stark.Starter.DDD.Infrastructure.SqlSugar;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Stark.Starter.DDD;

/// <summary>
/// 这里建个模块，主要为了EFCore上下文自动拥有属性注入的的功能
/// </summary>
[DependsOn(
    typeof(AbpAutofacModule)
)]
public class StarkStarterDDD : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);
        context.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
        // 获取配置
        var configuration = context.Services.GetConfiguration();
        
        // 获取数据库连接字符串
        var connectionString = configuration.GetConnectionString("ConnectionConfigs");
        Volo.Abp.Check.NotNull(connectionString, nameof (connectionString));
        // 获取数据库类型
        var dbType = configuration.GetConnectionString("DbType");
        Volo.Abp.Check.NotNull(dbType, nameof (dbType));
        
        // 添加SqlSugar
        context.Services.AddSqlSugar(() => new ConnectionConfig
        {
            ConnectionString = connectionString,
            DbType = (DbType)Enum.Parse(typeof(DbType), dbType)
        });
    }
}