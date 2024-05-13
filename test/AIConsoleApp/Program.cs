// See https://aka.ms/new-console-template for more information

using AIModule;
using Azure.AI.OpenAI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.TextGeneration;

var handler = new OpenAiHttpClientHandler();
 //
 // var builder = Kernel.CreateBuilder()
 //     .AddOpenAIChatCompletion(
 //         modelId: "gpt-3.5-turbo", //ERNIE-Bot    qwen-plus
 //         apiKey: "");

 var builder = Kernel.CreateBuilder();
 
 TestClient testClient = new TestClient();
 builder.AddOpenAIChatCompletion(
     modelId: "glm-3-turbo", //ERNIE-Bot    qwen-plus glm-3-turbo SparkDesk
    // apiKey: "",
     openAIClient:testClient
     );

builder.Plugins.AddFromType<TimeInformation>();
builder.Plugins.AddFromType<TextSkill>();

// builder.Services.AddKeyedSingleton<ITextGenerationService>("mock", new MockTextCompletion());
// builder.Services.AddKeyedSingleton<IChatCompletionService>("mock", new SparkDeskTextCompletion(new SparkDeskOptions(),"123"));


var kernel = builder.Build();



// Get chat completion service
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Start the conversation
while (true)
{
    // Create chat history
    ChatHistory history = new ChatHistory();
    // Get user input
    Console.Write("User > ");
    history.AddUserMessage(Console.ReadLine()!);

    // Enable auto function calling
    OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
    {
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
        Temperature = 0.5f
    };

    try
    {
        // Get the response from the AI
        var result = await chatCompletionService.GetChatMessageContentAsync(
            history,
            executionSettings: openAIPromptExecutionSettings,
            kernel: kernel);
        
        // Print the results
        Console.WriteLine("Bot > " + result);
        
        //Add the message from the agent to the chat history
        //.AddMessage(result.Role, result.Content);
        
        {
            // var chatResult =  chatCompletionService.GetStreamingChatMessageContentsAsync(
            //     history,
            //     executionSettings: openAIPromptExecutionSettings,
            //     kernel: kernel);
            //
            // Console.WriteLine("Bot > ");
            // await foreach (var content in chatResult)
            // {
            //     Console.Write(content);
            //     await Task.Delay(TimeSpan.FromMilliseconds(50));
            // }
        }
      

      
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

//注入SK插件
// builder.Plugins.AddFromType<TimeInformation>();
//
// var kernel = builder.Build();
// var prompt = @$"<message role=""user"">当前时间 {{TimeInformation.GetCurrentUtcTime}}. </message>";
// Console.WriteLine("结果：  " + await kernel.InvokePromptAsync(prompt));