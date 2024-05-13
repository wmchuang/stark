namespace AIModule;

public class OpenAiHttpClientHandler : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync(await request.Content.ReadAsStringAsync(cancellationToken));
        if (request.RequestUri.LocalPath == "/v1/chat/completions")
        {
            UriBuilder uriBuilder = new UriBuilder(request.RequestUri)
            {
                // 这里是你要修改的 URL
                Scheme = "http",
                Host = "101.133.236.73",
                Port = 13308,
            };
            request.RequestUri = uriBuilder.Uri;
        }
        
        Console.WriteLine(request.Content);

        // 接着，调用基类的 SendAsync 方法将你的修改后的请求发出去
        var response = await base.SendAsync(request, cancellationToken);
       // await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync(cancellationToken));
        return response;
    }
    
    
}