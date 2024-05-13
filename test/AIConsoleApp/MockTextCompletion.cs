// using System.Diagnostics;
// using System.Text;
// using System.Text.Encodings.Web;
// using System.Text.Json;
// using System.Text.Json.Serialization;
// using System.Text.Unicode;
// using Azure.AI.OpenAI;
// using Microsoft.Extensions.Logging;
// using Microsoft.SemanticKernel;
// using Microsoft.SemanticKernel.ChatCompletion;
// using Microsoft.SemanticKernel.Connectors.OpenAI;
// using Microsoft.SemanticKernel.Services;
// using Microsoft.SemanticKernel.TextGeneration;
//
// namespace AIConsoleApp
// {
//     public class MockTextCompletion : ITextGenerationService, IChatCompletionService
//     {
//         private readonly Dictionary<string, object?> _attributes = new();
//         private string _chatId;
//
//         private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
//         {
//             NumberHandling = JsonNumberHandling.AllowReadingFromString,
//             Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
//         };
//
//         public IReadOnlyDictionary<string, object?> Attributes => _attributes;
//
//         public MockTextCompletion()
//         {
//         }
//
//
//         public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(
//             ChatHistory chat,
//             PromptExecutionSettings? executionSettings,
//             Kernel? kernel,
//             CancellationToken cancellationToken = default)
//         {
//             // Convert the incoming execution settings to OpenAI settings.
//             OpenAIPromptExecutionSettings chatExecutionSettings =
//                 OpenAIPromptExecutionSettings.FromExecutionSettings(executionSettings);
//
//             // Create the Azure SDK ChatCompletionOptions instance from all available information.
//             var chatOptions =
//                 CreateChatCompletionsOptions(chatExecutionSettings, chat, kernel, this.DeploymentOrModelName);
//
//             for (int iteration = 1;; iteration++)
//             {
//                 // Make the request.
//                 var responseData =
//                     (await RunRequestAsync(() => this.Client.GetChatCompletionsAsync(chatOptions, cancellationToken))
//                         .ConfigureAwait(false)).Value;
//                 this.CaptureUsageDetails(responseData.Usage);
//                 if (responseData.Choices.Count == 0)
//                 {
//                     throw new KernelException("Chat completions not found");
//                 }
//
//                 // If we don't want to attempt to invoke any functions, just return the result.
//                 // Or if we are auto-invoking but we somehow end up with other than 1 choice even though only 1 was requested, similarly bail.
//                 if (!autoInvoke || responseData.Choices.Count != 1)
//                 {
//                     return responseData.Choices.Select(chatChoice => new OpenAIChatMessageContent(chatChoice.Message,
//                         this.DeploymentOrModelName, GetChatChoiceMetadata(responseData, chatChoice))).ToList();
//                 }
//
//                 Debug.Assert(kernel is not null);
//
//                 // Get our single result and extract the function call information. If this isn't a function call, or if it is
//                 // but we're unable to find the function or extract the relevant information, just return the single result.
//                 // Note that we don't check the FinishReason and instead check whether there are any tool calls, as the service
//                 // may return a FinishReason of "stop" even if there are tool calls to be made, in particular if a required tool
//                 // is specified.
//                 ChatChoice resultChoice = responseData.Choices[0];
//                 OpenAIChatMessageContent result = new(resultChoice.Message, this.DeploymentOrModelName,
//                     GetChatChoiceMetadata(responseData, resultChoice));
//                 if (result.ToolCalls.Count == 0)
//                 {
//                     return new[] { result };
//                 }
//
//                 if (this.Logger.IsEnabled(LogLevel.Debug))
//                 {
//                     this.Logger.LogDebug("Tool requests: {Requests}", result.ToolCalls.Count);
//                 }
//
//                 if (this.Logger.IsEnabled(LogLevel.Trace))
//                 {
//                     this.Logger.LogTrace("Function call requests: {Requests}",
//                         string.Join(", ",
//                             result.ToolCalls.OfType<ChatCompletionsFunctionToolCall>()
//                                 .Select(ftc => $"{ftc.Name}({ftc.Arguments})")));
//                 }
//
//                 // Add the original bot message to the chatOptions; this is required for the service
//                 // to understand the tool call responses. Also add the result message to the caller's chat
//                 // history: if they don't want it, they can remove it, but this makes the data available,
//                 // including metadata like usage.
//                 chatOptions.Messages.Add(GetRequestMessage(resultChoice.Message));
//                 chat.Add(result);
//
//                 // We must send back a response for every tool call, regardless of whether we successfully executed it or not.
//                 // If we successfully execute it, we'll add the result. If we don't, we'll add an error.
//                 for (int i = 0; i < result.ToolCalls.Count; i++)
//                 {
//                     ChatCompletionsToolCall toolCall = result.ToolCalls[i];
//
//                     // We currently only know about function tool calls. If it's anything else, we'll respond with an error.
//                     if (toolCall is not ChatCompletionsFunctionToolCall functionToolCall)
//                     {
//                         AddResponseMessage(chatOptions, chat, result: null, "Error: Tool call was not a function call.",
//                             toolCall.Id, this.Logger);
//                         continue;
//                     }
//
//                     // Parse the function call arguments.
//                     OpenAIFunctionToolCall? openAIFunctionToolCall;
//                     try
//                     {
//                         openAIFunctionToolCall = new(functionToolCall);
//                     }
//                     catch (JsonException)
//                     {
//                         AddResponseMessage(chatOptions, chat, result: null,
//                             "Error: Function call arguments were invalid JSON.", toolCall.Id, this.Logger);
//                         continue;
//                     }
//
//                     // Make sure the requested function is one we requested. If we're permitting any kernel function to be invoked,
//                     // then we don't need to check this, as it'll be handled when we look up the function in the kernel to be able
//                     // to invoke it. If we're permitting only a specific list of functions, though, then we need to explicitly check.
//                     if (chatExecutionSettings.ToolCallBehavior?.AllowAnyRequestedKernelFunction is not true &&
//                         !IsRequestableTool(chatOptions, openAIFunctionToolCall))
//                     {
//                         AddResponseMessage(chatOptions, chat, result: null,
//                             "Error: Function call request for a function that wasn't defined.", toolCall.Id,
//                             this.Logger);
//                         continue;
//                     }
//
//                     // Find the function in the kernel and populate the arguments.
//                     if (!kernel!.Plugins.TryGetFunctionAndArguments(openAIFunctionToolCall,
//                             out KernelFunction? function, out KernelArguments? functionArgs))
//                     {
//                         AddResponseMessage(chatOptions, chat, result: null,
//                             "Error: Requested function could not be found.", toolCall.Id, this.Logger);
//                         continue;
//                     }
//
//                     // Now, invoke the function, and add the resulting tool call message to the chat options.
//                     s_inflightAutoInvokes.Value++;
//                     object? functionResult;
//                     try
//                     {
//                         // Note that we explicitly do not use executionSettings here; those pertain to the all-up operation and not necessarily to any
//                         // further calls made as part of this function invocation. In particular, we must not use function calling settings naively here,
//                         // as the called function could in turn telling the model about itself as a possible candidate for invocation.
//                         functionResult =
//                             (await function.InvokeAsync(kernel, functionArgs, cancellationToken: cancellationToken)
//                                 .ConfigureAwait(false)).GetValue<object>() ?? string.Empty;
//                     }
// #pragma warning disable CA1031 // Do not catch general exception types
//                     catch (Exception e)
// #pragma warning restore CA1031
//                     {
//                         AddResponseMessage(chatOptions, chat, null,
//                             $"Error: Exception while invoking function. {e.Message}", toolCall.Id, this.Logger);
//                         continue;
//                     }
//                     finally
//                     {
//                         s_inflightAutoInvokes.Value--;
//                     }
//
//                     var stringResult = ProcessFunctionResult(functionResult, chatExecutionSettings.ToolCallBehavior);
//
//                     AddResponseMessage(chatOptions, chat, stringResult, errorMessage: null, toolCall.Id, this.Logger);
//
//                     static void AddResponseMessage(ChatCompletionsOptions chatOptions, ChatHistory chat, string? result,
//                         string? errorMessage, string toolId, ILogger logger)
//                     {
//                         // Log any error
//                         if (errorMessage is not null && logger.IsEnabled(LogLevel.Debug))
//                         {
//                             Debug.Assert(result is null);
//                             logger.LogDebug("Failed to handle tool request ({ToolId}). {Error}", toolId, errorMessage);
//                         }
//
//                         // Add the tool response message to both the chat options and to the chat history.
//                         result ??= errorMessage ?? string.Empty;
//                         chatOptions.Messages.Add(new ChatRequestToolMessage(result, toolId));
//                         chat.AddMessage(AuthorRole.Tool, result,
//                             metadata: new Dictionary<string, object?>
//                                 { { OpenAIChatMessageContent.ToolIdProperty, toolId } });
//                     }
//                 }
//
//                 // Update tool use information for the next go-around based on having completed another iteration.
//                 Debug.Assert(chatExecutionSettings.ToolCallBehavior is not null);
//
//                 // Set the tool choice to none. If we end up wanting to use tools, we'll reset it to the desired value.
//                 chatOptions.ToolChoice = ChatCompletionsToolChoice.None;
//                 chatOptions.Tools.Clear();
//
//                 if (iteration >= chatExecutionSettings.ToolCallBehavior!.MaximumUseAttempts)
//                 {
//                     // Don't add any tools as we've reached the maximum attempts limit.
//                     if (this.Logger.IsEnabled(LogLevel.Debug))
//                     {
//                         this.Logger.LogDebug("Maximum use ({MaximumUse}) reached; removing the tool.",
//                             chatExecutionSettings.ToolCallBehavior!.MaximumUseAttempts);
//                     }
//                 }
//                 else
//                 {
//                     // Regenerate the tool list as necessary. The invocation of the function(s) could have augmented
//                     // what functions are available in the kernel.
//                     chatExecutionSettings.ToolCallBehavior.ConfigureOptions(kernel, chatOptions);
//                 }
//
//                 // Having already sent tools and with tool call information in history, the service can become unhappy ("[] is too short - 'tools'")
//                 // if we don't send any tools in subsequent requests, even if we say not to use any.
//                 if (chatOptions.ToolChoice == ChatCompletionsToolChoice.None)
//                 {
//                     Debug.Assert(chatOptions.Tools.Count == 0);
//                     chatOptions.Tools.Add(s_nonInvocableFunctionTool);
//                 }
//
//                 // Disable auto invocation if we've exceeded the allowed limit.
//                 if (iteration >= chatExecutionSettings.ToolCallBehavior!.MaximumAutoInvokeAttempts)
//                 {
//                     autoInvoke = false;
//                     if (this.Logger.IsEnabled(LogLevel.Debug))
//                     {
//                         this.Logger.LogDebug("Maximum auto-invoke ({MaximumAutoInvoke}) reached.",
//                             chatExecutionSettings.ToolCallBehavior!.MaximumAutoInvokeAttempts);
//                     }
//                 }
//             }
//         }
//
//         public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
//             ChatHistory chatHistory,
//             PromptExecutionSettings? executionSettings = null, Kernel? kernel = null,
//             CancellationToken cancellationToken = new CancellationToken())
//         {
//             throw new NotImplementedException();
//         }
//
//         private static ChatCompletionsOptions CreateChatCompletionsOptions(
//             OpenAIPromptExecutionSettings executionSettings,
//             ChatHistory chatHistory,
//             Kernel? kernel,
//             string deploymentOrModelName)
//         {
//             if (executionSettings.ResultsPerPrompt is < 1 or > MaxResultsPerPrompt)
//             {
//                 throw new ArgumentOutOfRangeException(
//                     $"{nameof(executionSettings)}.{nameof(executionSettings.ResultsPerPrompt)}",
//                     executionSettings.ResultsPerPrompt,
//                     $"The value must be in range between 1 and {MaxResultsPerPrompt}, inclusive.");
//             }
//
//             var options = new ChatCompletionsOptions
//             {
//                 MaxTokens = executionSettings.MaxTokens,
//                 Temperature = (float?)executionSettings.Temperature,
//                 NucleusSamplingFactor = (float?)executionSettings.TopP,
//                 FrequencyPenalty = (float?)executionSettings.FrequencyPenalty,
//                 PresencePenalty = (float?)executionSettings.PresencePenalty,
//                 ChoiceCount = executionSettings.ResultsPerPrompt,
//                 DeploymentName = deploymentOrModelName,
//                 Seed = executionSettings.Seed,
//                 User = executionSettings.User
//             };
//
//             switch (executionSettings.ResponseFormat)
//             {
//                 case ChatCompletionsResponseFormat formatObject:
//                     // If the response format is an Azure SDK ChatCompletionsResponseFormat, just pass it along.
//                     options.ResponseFormat = formatObject;
//                     break;
//
//                 case string formatString:
//                     // If the response format is a string, map the ones we know about, and ignore the rest.
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
//
//                     break;
//
//                 case JsonElement formatElement:
//                     // This is a workaround for a type mismatch when deserializing a JSON into an object? type property.
//                     // Handling only string formatElement.
//                     if (formatElement.ValueKind == JsonValueKind.String)
//                     {
//                         string formatString = formatElement.GetString() ?? "";
//                         switch (formatString)
//                         {
//                             case "json_object":
//                                 options.ResponseFormat = ChatCompletionsResponseFormat.JsonObject;
//                                 break;
//
//                             case "text":
//                                 options.ResponseFormat = ChatCompletionsResponseFormat.Text;
//                                 break;
//                         }
//                     }
//
//                     break;
//             }
//
//             executionSettings.ToolCallBehavior?.ConfigureOptions(kernel, options);
//             if (executionSettings.TokenSelectionBiases is not null)
//             {
//                 foreach (var keyValue in executionSettings.TokenSelectionBiases)
//                 {
//                     options.TokenSelectionBiases.Add(keyValue.Key, keyValue.Value);
//                 }
//             }
//
//             if (executionSettings.StopSequences is { Count: > 0 })
//             {
//                 foreach (var s in executionSettings.StopSequences)
//                 {
//                     options.StopSequences.Add(s);
//                 }
//             }
//
//             if (!string.IsNullOrWhiteSpace(executionSettings?.ChatSystemPrompt) &&
//                 !chatHistory.Any(m => m.Role == AuthorRole.System))
//             {
//                 options.Messages.Add(GetRequestMessage(new ChatMessageContent(AuthorRole.System,
//                     executionSettings!.ChatSystemPrompt)));
//             }
//
//             foreach (var message in chatHistory)
//             {
//                 options.Messages.Add(GetRequestMessage(message));
//             }
//
//             return options;
//         }
//
//         private static ChatRequestMessage GetRequestMessage(ChatRole chatRole, string contents,
//             ChatCompletionsFunctionToolCall[]? tools)
//         {
//             if (chatRole == ChatRole.User)
//             {
//                 return new ChatRequestUserMessage(contents);
//             }
//
//             if (chatRole == ChatRole.System)
//             {
//                 return new ChatRequestSystemMessage(contents);
//             }
//
//             if (chatRole == ChatRole.Bot)
//             {
//                 var msg = new ChatRequestBotMessage(contents);
//                 if (tools is not null)
//                 {
//                     foreach (ChatCompletionsFunctionToolCall tool in tools)
//                     {
//                         msg.ToolCalls.Add(tool);
//                     }
//                 }
//
//                 return msg;
//             }
//
//             throw new NotImplementedException($"Role {chatRole} is not implemented");
//         }
//     }
// }