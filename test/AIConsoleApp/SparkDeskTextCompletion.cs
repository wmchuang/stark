// using System.Text.Json;
// using AIConsoleApp;
// using Azure.AI.OpenAI;
// using Microsoft.SemanticKernel;
// using Microsoft.SemanticKernel.ChatCompletion;
// using Microsoft.SemanticKernel.Connectors.OpenAI;
// using Sdcb.SparkDesk;
//
// namespace Stark.Module.AI;
//
// public class SparkDeskTextCompletion : IChatCompletionService
// {
//     private const int MaxResultsPerPrompt = 128;
//     private const int MaxInflightAutoInvokes = 5;
//     private static readonly ChatCompletionsFunctionToolDefinition s_nonInvocableFunctionTool = new() { Name = "NonInvocableTool" };
//     private static readonly AsyncLocal<int> s_inflightAutoInvokes = new();
//     public IReadOnlyDictionary<string, object> Attributes { get; }
//
//     private readonly SparkDeskClient _client;
//     private string _chatId;
//     private readonly SparkDeskOptions _options;
//
//     public SparkDeskTextCompletion(SparkDeskOptions options, string chatId)
//     {
//         _options = options;
//         _chatId = chatId;
//         _client = new(options.AppId, options.ApiKey, options.ApiSecret);
//     }
//
//     /// <summary>
//     /// Model Id or Deployment Name
//     /// </summary>
//     internal string DeploymentOrModelName { get; set; } = string.Empty;
//
//     public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chat, PromptExecutionSettings executionSettings = null, Kernel kernel = null,
//         CancellationToken cancellationToken = new CancellationToken())
//     {
//         OpenAIPromptExecutionSettings chatExecutionSettings = OpenAIPromptExecutionSettings.FromExecutionSettings(executionSettings);
//         bool autoInvoke = kernel is not null && chatExecutionSettings.ToolCallBehavior == ToolCallBehavior.AutoInvokeKernelFunctions;
//         // Create the Azure SDK ChatCompletionOptions instance from all available information.
//         var chatOptions = CreateChatCompletionsOptions(chatExecutionSettings, chat, kernel, this.DeploymentOrModelName);
//
//         // ChatResponse response = await _client.ChatAsync(ModelVersion.V1_5, new ChatMessage[]
//         // {
//         //     ChatMessage.FromUser("系统提示：你叫张三，一名5岁男孩，你在金色摇篮幼儿园上学，你的妈妈叫李四，是一名工程师"),
//         //     ChatMessage.FromUser("你好小朋友，我是周老师，你在哪上学？"),
//         // }, cancellationToken: cancellationToken);
//         // Console.WriteLine(response.Text);
//         //
//         // var messages = chatOptions.Messages.Select(x =>
//         // {
//         //     if (x is ChatRequestUserMessage userMsg)
//         //     {
//         //         return new ChatMessage(userMsg.Content);
//         //     }
//         //
//         //     return "";
//         // }).ToList();
//         //
//         // var parameters = new ChatRequestParameters
//         // {
//         //     ChatId = _chatId,
//         // };
//         // parameters.Temperature = (float)chatExecutionSettings.Temperature;
//         // parameters.MaxTokens = chatExecutionSettings.MaxTokens ?? parameters.MaxTokens;
//         // var functionDefs = chatOptions.Tools.Where(x => x is ChatCompletionsFunctionToolCall).Select(x =>
//         // {
//         //     var func = x as ChatCompletionsFunctionToolDefinition;
//         //     return new FunctionDef(func.Name, func.Description,
//         //         func.Parameters.Select(p => new FunctionParametersDef(p.Name, p.ParameterType?.IsClass == true ? "object" : "string", func.Description, p.IsRequired)).ToList())
//         // }).ToList();
//         //
//         // await foreach (StreamedChatResponse msg in _client.ChatAsync(_options.ModelVersion, messages, parameters, functionDefs.Count > 0 ? [.. functionDefs] : null,
//         //                    cancellationToken: cancellationToken))
//         // {
//         // }
//
//         return null;
//     }
//
//     public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings executionSettings = null, Kernel kernel = null,
//         CancellationToken cancellationToken = new CancellationToken())
//     {
//         throw new NotImplementedException();
//     }
//
//     private static CompletionsOptions CreateCompletionsOptions(string text, OpenAIPromptExecutionSettings executionSettings, string deploymentOrModelName)
//     {
//         if (executionSettings.ResultsPerPrompt is < 1 or > MaxResultsPerPrompt)
//         {
//             throw new ArgumentOutOfRangeException($"{nameof(executionSettings)}.{nameof(executionSettings.ResultsPerPrompt)}", executionSettings.ResultsPerPrompt,
//                 $"The value must be in range between 1 and {MaxResultsPerPrompt}, inclusive.");
//         }
//
//         var options = new CompletionsOptions
//         {
//             Prompts = { text.Replace("\r\n", "\n") }, // normalize line endings
//             MaxTokens = executionSettings.MaxTokens,
//             Temperature = (float?)executionSettings.Temperature,
//             NucleusSamplingFactor = (float?)executionSettings.TopP,
//             FrequencyPenalty = (float?)executionSettings.FrequencyPenalty,
//             PresencePenalty = (float?)executionSettings.PresencePenalty,
//             Echo = false,
//             ChoicesPerPrompt = executionSettings.ResultsPerPrompt,
//             GenerationSampleCount = executionSettings.ResultsPerPrompt,
//             LogProbabilityCount = null,
//             User = executionSettings.User,
//             DeploymentName = deploymentOrModelName
//         };
//
//         if (executionSettings.TokenSelectionBiases is not null)
//         {
//             foreach (var keyValue in executionSettings.TokenSelectionBiases)
//             {
//                 options.TokenSelectionBiases.Add(keyValue.Key, keyValue.Value);
//             }
//         }
//
//         if (executionSettings.StopSequences is { Count: > 0 })
//         {
//             foreach (var s in executionSettings.StopSequences)
//             {
//                 options.StopSequences.Add(s);
//             }
//         }
//
//         return options;
//     }
//
//     private static ChatCompletionsOptions CreateChatCompletionsOptions(
//         OpenAIPromptExecutionSettings executionSettings,
//         ChatHistory chatHistory,
//         Kernel? kernel,
//         string deploymentOrModelName)
//     {
//         if (executionSettings.ResultsPerPrompt is < 1 or > MaxResultsPerPrompt)
//         {
//             throw new ArgumentOutOfRangeException($"{nameof(executionSettings)}.{nameof(executionSettings.ResultsPerPrompt)}", executionSettings.ResultsPerPrompt,
//                 $"The value must be in range between 1 and {MaxResultsPerPrompt}, inclusive.");
//         }
//
//         var options = new ChatCompletionsOptions
//         {
//             MaxTokens = executionSettings.MaxTokens,
//             Temperature = (float?)executionSettings.Temperature,
//             NucleusSamplingFactor = (float?)executionSettings.TopP,
//             FrequencyPenalty = (float?)executionSettings.FrequencyPenalty,
//             PresencePenalty = (float?)executionSettings.PresencePenalty,
//             ChoiceCount = executionSettings.ResultsPerPrompt,
//             DeploymentName = deploymentOrModelName,
//             Seed = executionSettings.Seed,
//             User = executionSettings.User
//         };
//
//         switch (executionSettings.ResponseFormat)
//         {
//             case ChatCompletionsResponseFormat formatObject:
//                 // If the response format is an Azure SDK ChatCompletionsResponseFormat, just pass it along.
//                 options.ResponseFormat = formatObject;
//                 break;
//
//             case string formatString:
//                 // If the response format is a string, map the ones we know about, and ignore the rest.
//                 switch (formatString)
//                 {
//                     case "json_object":
//                         options.ResponseFormat = ChatCompletionsResponseFormat.JsonObject;
//                         break;
//
//                     case "text":
//                         options.ResponseFormat = ChatCompletionsResponseFormat.Text;
//                         break;
//                 }
//
//                 break;
//
//             case JsonElement formatElement:
//                 // This is a workaround for a type mismatch when deserializing a JSON into an object? type property.
//                 // Handling only string formatElement.
//                 if (formatElement.ValueKind == JsonValueKind.String)
//                 {
//                     string formatString = formatElement.GetString() ?? "";
//                     switch (formatString)
//                     {
//                         case "json_object":
//                             options.ResponseFormat = ChatCompletionsResponseFormat.JsonObject;
//                             break;
//
//                         case "text":
//                             options.ResponseFormat = ChatCompletionsResponseFormat.Text;
//                             break;
//                     }
//                 }
//
//                 break;
//         }
//
//         // executionSettings.ToolCallBehavior?.ConfigureOptions(kernel, options);
//
//         if (kernel is not null)
//         {
//             // Provide all functions from the kernel.
//             IList<KernelFunctionMetadata> functions = kernel.Plugins.GetFunctionsMetadata();
//             if (functions.Count > 0)
//             {
//                 options.ToolChoice = ChatCompletionsToolChoice.Auto;
//                 for (int i = 0; i < functions.Count; i++)
//                 {
//                     options.Tools.Add(new ChatCompletionsFunctionToolDefinition(functions[i].ToOpenAIFunction().ToFunctionDefinition()));
//                 }
//             }
//         }
//
//         if (executionSettings.TokenSelectionBiases is not null)
//         {
//             foreach (var keyValue in executionSettings.TokenSelectionBiases)
//             {
//                 options.TokenSelectionBiases.Add(keyValue.Key, keyValue.Value);
//             }
//         }
//
//         if (executionSettings.StopSequences is { Count: > 0 })
//         {
//             foreach (var s in executionSettings.StopSequences)
//             {
//                 options.StopSequences.Add(s);
//             }
//         }
//
//         if (!string.IsNullOrWhiteSpace(executionSettings?.ChatSystemPrompt) && !chatHistory.Any(m => m.Role == AuthorRole.System))
//         {
//             options.Messages.Add(GetRequestMessage(new ChatMessageContent(AuthorRole.System, executionSettings!.ChatSystemPrompt)));
//         }
//
//         foreach (var message in chatHistory)
//         {
//             options.Messages.Add(GetRequestMessage(message));
//         }
//
//         return options;
//     }
//
//     private static ChatRequestMessage GetRequestMessage(ChatMessageContent message)
//     {
//         if (message.Role == AuthorRole.System)
//         {
//             return new ChatRequestSystemMessage(message.Content);
//         }
//
//         if (message.Role == AuthorRole.User || message.Role == AuthorRole.Tool)
//         {
//             if (message.Metadata?.TryGetValue(OpenAIChatMessageContent.ToolIdProperty, out object? toolId) is true &&
//                 toolId?.ToString() is string toolIdString)
//             {
//                 return new ChatRequestToolMessage(message.Content, toolIdString);
//             }
//
//             if (message.Items is { Count: 1 } && message.Items.FirstOrDefault() is TextContent textContent)
//             {
//                 return new ChatRequestUserMessage(textContent.Text);
//             }
//
//             return new ChatRequestUserMessage(message.Items.Select(static (KernelContent item) => (ChatMessageContentItem)(item switch
//             {
//                 TextContent textContent => new ChatMessageTextContentItem(textContent.Text),
//                 ImageContent imageContent => new ChatMessageImageContentItem(imageContent.Uri),
//                 _ => throw new NotSupportedException($"Unsupported chat message content type '{item.GetType()}'.")
//             })));
//         }
//
//         // if (message.Role == AuthorRole.Bot)
//         // {
//         //     var asstMessage = new ChatRequestBotMessage(message.Content);
//         //
//         //     IEnumerable<ChatCompletionsToolCall>? tools = (message as OpenAIChatMessageContent)?.ToolCalls;
//         //     if (tools is null && message.Metadata?.TryGetValue(OpenAIChatMessageContent.FunctionToolCallsProperty, out object? toolCallsObject) is true)
//         //     {
//         //         tools = toolCallsObject as IEnumerable<ChatCompletionsFunctionToolCall>;
//         //         if (tools is null && toolCallsObject is JsonElement { ValueKind: JsonValueKind.Array } array)
//         //         {
//         //             int length = array.GetArrayLength();
//         //             var ftcs = new List<ChatCompletionsToolCall>(length);
//         //             for (int i = 0; i < length; i++)
//         //             {
//         //                 JsonElement e = array[i];
//         //                 if (e.TryGetProperty("Id", out JsonElement id) &&
//         //                     e.TryGetProperty("Name", out JsonElement name) &&
//         //                     e.TryGetProperty("Arguments", out JsonElement arguments) &&
//         //                     id.ValueKind == JsonValueKind.String &&
//         //                     name.ValueKind == JsonValueKind.String &&
//         //                     arguments.ValueKind == JsonValueKind.String)
//         //                 {
//         //                     ftcs.Add(new ChatCompletionsFunctionToolCall(id.GetString()!, name.GetString()!, arguments.GetString()!));
//         //                 }
//         //             }
//         //
//         //             tools = ftcs;
//         //         }
//         //     }
//         //
//         //     if (tools is not null)
//         //     {
//         //         foreach (ChatCompletionsToolCall tool in tools)
//         //         {
//         //             asstMessage.ToolCalls.Add(tool);
//         //         }
//         //     }
//         //
//         //     return asstMessage;
//         // }
//
//         throw new NotSupportedException($"Role {message.Role} is not supported.");
//     }
// }