using Microsoft.SemanticKernel;
using Stark.Module.AI.Domain;
using Stark.Module.AI.Models.Enum;

namespace Stark.Module.AI.Application.Sk;

public static class IKernelBuilderExtensions
{
    public static IKernelBuilder AddChatCompletion(this IKernelBuilder builder, AiModel model)
    {
        switch (model.Type)
        {
            case AiTypeEnum.OpenAi:
                var client = OpenAiHttpClientHandler.GetHttpClient(model.EndPoint);
                builder.AddOpenAIChatCompletion(
                    modelId: model.ModelName,
                    apiKey: model.ModelKey,
                    httpClient: client);
                break;
            case AiTypeEnum.AzureOpenAi:
                builder.AddAzureOpenAIChatCompletion(
                    deploymentName: model.ModelName,
                    apiKey: model.ModelKey,
                    endpoint: model.EndPoint
                );
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return builder;
    }
}