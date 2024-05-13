using Microsoft.KernelMemory;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;
using Microsoft.KernelMemory.Postgres;
using Stark.Module.AI.Domain;
using Stark.Module.AI.Models.Enum;

namespace Stark.Module.AI.Application.Sk;

public static class IKernelMemoryBuilderExtensions
{
    /// <summary>
    /// 配置文本生成模型
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="chatModel"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IKernelMemoryBuilder WithTextGeneration(this IKernelMemoryBuilder builder, AiModel chatModel)
    {
        switch (chatModel.Type)
        {
            case AiTypeEnum.OpenAi:
                var client = OpenAiHttpClientHandler.GetHttpClient(chatModel.EndPoint);
                builder.WithOpenAITextGeneration(new OpenAIConfig()
                {
                    APIKey = chatModel.ModelKey,
                    TextModel = chatModel.ModelName
                }, null, client);
                break;
            case AiTypeEnum.AzureOpenAi:
                builder.WithAzureOpenAITextGeneration(new AzureOpenAIConfig()
                {
                    APIKey = chatModel.ModelKey,
                    Deployment = chatModel.ModelName,
                    Endpoint = chatModel.EndPoint,
                    Auth = AzureOpenAIConfig.AuthTypes.APIKey,
                    APIType = AzureOpenAIConfig.APITypes.TextCompletion,
                });
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return builder;
    }

    /// <summary>
    /// 配置文档解析向量模型
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="embeddingModel"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IKernelMemoryBuilder WithTextEmbeddingGeneration(this IKernelMemoryBuilder builder, AiModel embeddingModel)
    {
        switch (embeddingModel.Type)
        {
            case AiTypeEnum.OpenAi:
                var client = OpenAiHttpClientHandler.GetHttpClient(embeddingModel.EndPoint);
                builder.WithOpenAITextEmbeddingGeneration(new OpenAIConfig()
                {
                    APIKey = embeddingModel.ModelKey,
                    EmbeddingModel = embeddingModel.ModelName
                }, null, false, client);
                break;
            case AiTypeEnum.AzureOpenAi:
                builder.WithAzureOpenAITextEmbeddingGeneration(new AzureOpenAIConfig()
                {
                    APIKey = embeddingModel.ModelKey,
                    Deployment = embeddingModel.ModelName,
                    Endpoint = embeddingModel.EndPoint,
                    Auth = AzureOpenAIConfig.AuthTypes.APIKey,
                    APIType = AzureOpenAIConfig.APITypes.TextCompletion,
                });
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return builder;
    }

    public static IKernelMemoryBuilder WithMemoryDb(this IKernelMemoryBuilder builder, AiWiki wiki)
    {
        switch (wiki.DbType)
        {
            case WikiSaveDbType.Postgres:
                builder.WithPostgresMemoryDb(new PostgresConfig()
                {
                    ConnectionString = wiki.ConnectionString,
                });
                break;
            case WikiSaveDbType.Disk:
                builder.WithSimpleVectorDb(new SimpleVectorDbConfig()
                {
                    StorageType = FileSystemTypes.Disk,
                    Directory = "wiki"
                });
                break;
            case WikiSaveDbType.Memory:
                builder.WithSimpleVectorDb(new SimpleVectorDbConfig()
                {
                    StorageType = FileSystemTypes.Volatile,
                });
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return builder;
    }
}