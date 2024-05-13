using System.Text;
using System.Text.Json;
using Azure;
using Azure.AI.OpenAI;
using Azure.Core;
using Azure.Core.Pipeline;
using Json.More;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AIModule;

public class TestClient : OpenAIClient
{
    private readonly HttpPipeline _pipeline;
    public  TestClient()
    {
        _pipeline = HttpPipelineBuilder.Build(new OpenAIClientOptions());
    }

    private static readonly HttpClient Client = new();
    /// <summary> The ClientDiagnostics is used to provide tracing support for the client library. </summary>

    /// <summary> The HTTP pipeline for sending and receiving REST requests and responses. </summary>
    public override Response<ChatCompletions> GetChatCompletions(ChatCompletionsOptions chatCompletionsOptions, CancellationToken cancellationToken = new CancellationToken())
    {
        return base.GetChatCompletions(chatCompletionsOptions, cancellationToken);
    }

    public override Response<Completions> GetCompletions(CompletionsOptions completionsOptions, CancellationToken cancellationToken = new CancellationToken())
    {
        return base.GetCompletions(completionsOptions, cancellationToken);
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };
    public override async Task<Response<ChatCompletions>> GetChatCompletionsAsync(ChatCompletionsOptions chatCompletionsOptions, CancellationToken cancellationToken = new CancellationToken())
    {
        
     var t =    chatCompletionsOptions.Messages.Select(msg =>
        {
            if (msg is ChatRequestUserMessage userMsg)
            {
                return userMsg.Content;
            }

            return null;
        }).ToList();
        
        var json = JsonSerializer.Serialize(chatCompletionsOptions, JsonOptions);
        
        var m = new Utf8JsonRequestContent();
        //将json转成流，写到m中
        m.JsonWriter.WriteObjectValue(chatCompletionsOptions);
      
        
        RequestContent content = m;
        RequestContext context = new RequestContext() { CancellationToken = cancellationToken };
        try
        {
            using HttpMessage message = CreatePostRequestMessage(chatCompletionsOptions, content, context);
            Response response = await _pipeline.ProcessMessageAsync(message, context, cancellationToken);
            
            var result = JsonSerializer.Deserialize<ChatCompletions>(response.Content);
            return Response.FromValue(result, response);
            
        }
        catch (Exception e)
        {
            throw;
        }
    }
    
    internal HttpMessage CreatePostRequestMessage(
        ChatCompletionsOptions chatCompletionsOptions,
        RequestContent content,
        RequestContext context)
    {
        string operationPath = "chat/completions";
        return CreatePostRequestMessage(chatCompletionsOptions.DeploymentName, operationPath, content, context);
    }
    private const int API_TOKEN_TTL_SECONDS = 60 * 5;
    private const string apiKey = "054a952c57695a27bddd58fbfbf6bfc0.HaVajzqH77ZtxvHE";
    
    internal HttpMessage CreatePostRequestMessage(
        string deploymentOrModelName,
        string operationPath,
        RequestContent content,
        RequestContext context)
    {
        var requestUri = new RequestUriBuilder();
        requestUri.Reset(new Uri("https://open.bigmodel.cn/api/paas/v4/chat/completions"));
        
        var token = string.IsNullOrEmpty(apiKey)
            ? string.Empty
            : AuthenticationUtils.GenerateToken(apiKey, API_TOKEN_TTL_SECONDS);
    
        HttpMessage message = _pipeline.CreateMessage(context, ResponseClassifier200);
        Request request = message.Request;
        request.Method = RequestMethod.Post;
        request.Uri = requestUri;
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Content-Type", "application/json");
        request.Headers.Add("Authorization", token);
        request.Content = content;
        return message;
    }
    
    
    private static ResponseClassifier _responseClassifier200;
    private static ResponseClassifier ResponseClassifier200 => _responseClassifier200 ??= new StatusCodeClassifier(stackalloc ushort[] { 200 });
}