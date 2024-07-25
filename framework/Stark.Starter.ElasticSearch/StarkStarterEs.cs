using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stark.Starter.ElasticSearch.Config;
using Stark.Starter.ElasticSearch.Context;
using Volo.Abp.Modularity;

namespace Stark.Starter.ElasticSearch;

public class StarkStarterEs : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var section = context.Services.GetConfiguration().GetSection("DbConfig:Elasticsearch");

        var exist = section.Exists();
        if (!exist)
        {
            throw new Exception("请在appsettings.json中配置DbConfig:Elasticsearch");
        }

        context.Services.Configure<EsOptions>(section);
        context.Services.AddSingleton<IElasticSearchContext, ElasticSearchContext>();
    }
}