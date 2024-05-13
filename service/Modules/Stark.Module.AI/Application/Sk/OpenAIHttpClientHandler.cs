using Stark.Module.AI.Options;

namespace Stark.Module.AI.Application.Sk;

public class OpenAiHttpClientHandler : HttpClientHandler
{
    private string _endPoint { get; set; }

    public OpenAiHttpClientHandler(string endPoint)
    {
        this._endPoint = endPoint;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        UriBuilder uriBuilder;
        if (!_endPoint.IsNullOrWhiteSpace() && request.RequestUri?.LocalPath == "/v1/chat/completions")
        {
            uriBuilder = new UriBuilder(_endPoint.TrimEnd('/') + "/v1/chat/completions");
            request.RequestUri = uriBuilder.Uri;
        }
        else if (!_endPoint.IsNullOrWhiteSpace() &&
                 request.RequestUri?.LocalPath == "/v1/embeddings")
        {
            uriBuilder = new UriBuilder(_endPoint.TrimEnd('/') + "/v1/embeddings");
            request.RequestUri = uriBuilder.Uri;
        }

        return await base.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 获取http客户端
    /// </summary>
    /// <param name="endPoint"></param>
    /// <returns></returns>
    public static HttpClient GetHttpClient(string endPoint)
    {
        var handler = new OpenAiHttpClientHandler(endPoint);
        var httpClient = new HttpClient(handler);
        httpClient.Timeout = TimeSpan.FromMinutes(1);
        return httpClient;
    }
}