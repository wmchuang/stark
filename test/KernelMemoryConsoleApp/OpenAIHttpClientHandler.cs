﻿namespace KernelMemoryConsoleApp;

/// <summary>
/// 重写SemanticKernel的请求处理
/// </summary>
public sealed class OpenAIHttpClientHandler : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        UriBuilder uriBuilder;
        if (request.RequestUri?.LocalPath == "/v1/chat/completions")
        {
            uriBuilder = new UriBuilder(OpenAIOption.ChatEndpoint.TrimEnd('/') + "/v1/chat/completions");
            request.RequestUri = uriBuilder.Uri;
        }
        else if (
                 request.RequestUri?.LocalPath == "/v1/embeddings")
        {
            uriBuilder = new UriBuilder(OpenAIOption.EmbeddingEndpoint.TrimEnd('/') + "/v1/embeddings");
            request.RequestUri = uriBuilder.Uri;
        }

        return await base.SendAsync(request, cancellationToken);
    }
}