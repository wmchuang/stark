using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stark.Starter.Mongodb.Config;
using Stark.Starter.Mongodb.Context;
using Volo.Abp.Modularity;

namespace Stark.Starter.Mongodb;

public class StarkStarterMongoDb : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var section = context.Services.GetConfiguration().GetSection("DbConfig:MongoDb");

        var exist = section.Exists();
        if (!exist)
        {
            throw new Exception("请在appsettings.json中配置DbConfig:MongoDb");
        }

        context.Services.Configure<MongoOptions>(section);
        
        context.Services.AddSingleton<IMongoContext, MongoContext>();

    }
}