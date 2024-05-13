using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Stark.Starter.Cap;

public class StarkStarterCap : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 获取配置
        var configuration = context.Services.GetConfiguration();

        // 获取数据库连接字符串
        var connectionString = configuration.GetConnectionString("ConnectionConfigs");
        Volo.Abp.Check.NotNull(connectionString, nameof(connectionString));

        context.Services.AddCap(option =>
        {
            option.UseMySql(connectionString!);
            option.UseDashboard();

            option.UseRabbitMQ(x => { configuration.GetSection("Cap:RabbitMQ").Bind(x); });
        });
    }
}