using Elastic.Clients.Elasticsearch;

namespace Stark.Starter.ElasticSearch.Context
{
    public interface IElasticSearchContext
    {
        ElasticsearchClient ElasticsearchClient { get; }
    }
}