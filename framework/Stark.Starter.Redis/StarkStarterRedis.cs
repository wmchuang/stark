using CSRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Stark.Starter.Redis;

public class StarkStarterRedis : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var config = context.Services.GetConfiguration().GetSection("Redis").Get<RedisConfig>();
        
        RedisHelper.Initialization(new CSRedisClient($"{config.ConnectionString},prefix={config.InstanceName},testcluster=false"));
    }
}