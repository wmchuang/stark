using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace StarkTest;

public class ESTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ESTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Test1()
    {
        var settings = new ElasticsearchClientSettings(new Uri("http://192.168.10.29:9200"));

        var client = new ElasticsearchClient(settings);

        client.Indices.ExistsAsync("twitter");
        
        // var tweet = new Tweet 
        // {
        //     Id = 2,
        //     User = "stevejgordon",
        //     PostDate = new DateTime(2009, 11, 15),
        //     Message = "Trying out the client, so far so good?"
        // };
        //
        //
        // var response = await client.IndexAsync(tweet, i => i.Index("twitter").Id(tweet.Id));

        var searchAsync = await client.SearchAsync<Tweet>(x => x.Index("twitter"));
        _testOutputHelper.WriteLine(searchAsync.Documents.First().Id.ToString());
    }
}