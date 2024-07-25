using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stark.Starter.ElasticSearch.Config;

namespace Stark.Starter.ElasticSearch.Context
{
    public class ElasticSearchContext: IElasticSearchContext
    {
        // 最好设置为单例注入
        public ElasticSearchContext(IOptions<EsOptions> options,
            ILogger<ElasticSearchContext> logger)
        {
            var connectionSetting = new ElasticsearchClientSettings();
            try
            {
                var uris = options.Value.Uris;
                if (uris == null || uris.Count < 1)
                {
                    throw new Exception("urls can not be null");
                }

                if (uris.Count == 1)
                {
                    var uri = uris.First();
                    connectionSetting = new ElasticsearchClientSettings(uri);
                }

                if (!string.IsNullOrWhiteSpace(options.Value.UserName) && !string.IsNullOrWhiteSpace(options.Value.Password))
                    connectionSetting.Authentication(new BasicAuthentication(options.Value.UserName, options.Value.Password));

                ElasticsearchClient = new ElasticsearchClient(connectionSetting);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ElasticSearch Initialized failed.");
                throw;
            }
        }

        public ElasticsearchClient ElasticsearchClient { get; }
    }
}