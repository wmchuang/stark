using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Newtonsoft.Json;
using Stark.Module.AI.Application.Service;
using Stark.Module.AI.Models.Cache;
using Stark.Module.AI.Models.OpenAPI;
using Stark.Module.AI.Models.Request;
using Stark.Starter.Redis;

namespace Stark.Module.AI.Application.Sk;

public class ChatService : IChatService
{
    private readonly ILogger<ChatService> _logger;
    private readonly IRedisCache _redisCache;
    private readonly IAiKernelService _aiKernelService;
    private readonly IAiKernelMemoryService _aiKernelMemoryService;
    private readonly AiBotService _botService;
    private readonly AiWikiService _wikiService;

    private const string _systemPrompt = @"你是一个大型语言模型，尽可能简洁地回答";

    private const string _answerPrompt = @"忘记你已有的知识，仅使用 <QA></QA> 标记中的问答对进行回答。
<QA>
{{quote}}
</QA>}
思考流程：
1. 仅鉴于上述事实，请提供全面/详细的答案。
1. 判断问题是否与 <QA></QA> 标记中的内容有关。
2. 如果无关，直接回复 知识库未搜索到相关内容。
3. 判断是否有相近或相同的问题。
4. 如果有相同的问题，直接输出对应答案。
5. 如果只有相近的问题，请把相近的问题和答案一起输出。

最后，避免提及你是从 QA 获取的知识，只需要回复答案。

问题:{{question}}`
  }";

    public ChatService(ILogger<ChatService> logger,IRedisCache redisCache, IAiKernelService aiKernelService, IAiKernelMemoryService aiKernelMemoryService, AiBotService botService, AiWikiService wikiService)
    {
        _logger = logger;
        _redisCache = redisCache;
        _aiKernelService = aiKernelService;
        _aiKernelMemoryService = aiKernelMemoryService;
        _botService = botService;
        _wikiService = wikiService;
    }

    /// <summary>
    /// 聊天
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ChatMessageContent?> ChatAsync(HttpContext httpContext, ChatRequest request)
    {
        var systemPrompt = _systemPrompt;

        var bot = await _botService.DetailAsync(request.BotId);
        if (!bot.WikiIds.IsNullOrWhiteSpace())
        {
            request.WikiId = JsonConvert.DeserializeObject<List<string>>(bot.WikiIds).First();
        }

        systemPrompt = bot.Prompting;

        var kernel = await _aiKernelService.GetKernel(bot);

        if (!request.WikiId.IsNullOrEmpty())
        {
            var wiki = await _wikiService.GetAsync(request.WikiId!);
            var sourceRelevantList = await _aiKernelMemoryService.SourceRelevantListAsync(wiki, request.Context);
            if (sourceRelevantList.Any())
            {
                var dataMsg = new StringBuilder();
                foreach (var sourceRelevant in sourceRelevantList)
                {
                    dataMsg.Append(sourceRelevant);
                }

                request.Context = _answerPrompt.Replace("{{quote}}", dataMsg.ToString())
                            .Replace("{{question}}", request.Context);

                _logger.LogInformation($"request.Context:{request.Context}");
            }
        }

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var history = await GetChatHistoryAsync(request);

        var executionSettings = new OpenAIPromptExecutionSettings
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
            Temperature = Convert.ToDouble(bot.Temperature),
            ChatSystemPrompt = systemPrompt
        };

        if (request.Stream)
        {
            OpenAIResult result = new OpenAIResult
            {
                created = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                choices = new List<ChoicesModel>()
                {
                    new ChoicesModel()
                    {
                        message = new OpenAIMessage()
                        {
                            role = AuthorRole.Assistant.ToString()
                        }
                    }
                }
            };

            var chatResult = chatCompletionService.GetStreamingChatMessageContentsAsync(history, executionSettings: executionSettings);
            await ResponseStreamAsync(httpContext, chatResult, result);

            return null;
        }
        else
        {
            var result = await chatCompletionService.GetChatMessageContentAsync(history, executionSettings: executionSettings);
            return result;
        }
    }

    private async Task<ChatHistory> GetChatHistoryAsync(ChatRequest request)
    {
        var history = new ChatHistory();

        if (request.ChatId.IsNullOrEmpty())
        {
            request.ChatId = Guid.NewGuid().ToString();
        }

        var redisData = await _redisCache.GetAsync<List<ChatHistoryModel>>("chat:" + request.ChatId);
        if (redisData?.Count > 0)
        {
            foreach (var item in redisData)
            {
                if (item.Role == AuthorRole.User)
                    history.AddUserMessage(item.Context);
                else
                    history.AddAssistantMessage(item.Context);
            }
        }
        else
        {
            redisData = new List<ChatHistoryModel>();
        }

        redisData.Add(new ChatHistoryModel()
        {
            Role = AuthorRole.User,
            Context = request.Context
        });

        history.AddUserMessage(request.Context);

        await _redisCache.SetAsync("chat:" + request.ChatId, redisData);

        return history;
    }

    /// <summary>
    /// 流式输出
    /// </summary>
    /// <param name="chatResult"></param>
    /// <param name="httpContext"></param>
    /// <param name="result"></param>
    private async Task ResponseStreamAsync(HttpContext httpContext,
        IAsyncEnumerable<StreamingChatMessageContent> chatResult,
        OpenAIResult result)
    {
        httpContext.Response!.Headers!.Add("Content-Type", "text/event-stream");

        await foreach (var content in chatResult)
        {
            result.choices[0].message.content = content.Content?.ToString();
            string message = $"data: {JsonConvert.SerializeObject(result)}\n\n";
            await httpContext.Response.WriteAsync(message, Encoding.UTF8);
            await httpContext.Response.Body.FlushAsync();
        }

        await httpContext.Response.WriteAsync("data: [DONE]");
        await httpContext.Response.Body.FlushAsync();

        await httpContext.Response.CompleteAsync();
    }
}