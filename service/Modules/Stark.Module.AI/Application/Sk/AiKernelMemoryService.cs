using Microsoft.KernelMemory;
using Microsoft.KernelMemory.Configuration;
using Microsoft.KernelMemory.MemoryStorage;
using Stark.Module.AI.Application.Service;
using Stark.Module.AI.Domain;
using Stark.Module.AI.Models.Dto;
using Stark.Module.AI.Models.Result;

namespace Stark.Module.AI.Application.Sk;

/// <summary>
/// kernel memory服务 
/// </summary>
public class AiKernelMemoryService : IAiKernelMemoryService
{
    private readonly AiModelService _modelService;

    public AiKernelMemoryService(AiModelService modelService)
    {
        _modelService = modelService;
    }

    /// <summary>
    /// 获取memory
    /// </summary>
    /// <returns></returns>
    public async Task<MemoryServerless> GetMemoryAsync(AiWiki aiWiki)
    {
        var chatModel = await _modelService.GetModelAsync(aiWiki.ChatModelId);
        var embeddingModel = await _modelService.GetModelAsync(aiWiki.EmbeddingModelId);

        try
        {
            var memoryBuilder = new KernelMemoryBuilder()
                .WithSearchClientConfig(new SearchClientConfig()
                {
                    //最大匹配数量
                    MaxMatchesCount = 5,
                    AnswerTokens = 100,
                    EmptyAnswer = "知识库未搜索到相关内容"
                })
                .With(new TextPartitioningOptions
                {
                    MaxTokensPerParagraph = 300,
                    MaxTokensPerLine = 100,
                    OverlappingTokens = 30
                })
                .WithMemoryDb(wiki: aiWiki)
                .WithTextGeneration(chatModel: chatModel)
                .WithTextEmbeddingGeneration(embeddingModel: embeddingModel);
            return memoryBuilder.Build<MemoryServerless>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<SourceRelevant>> SourceRelevantListAsync(AiWiki wiki, string context)
    {
        var result = new List<SourceRelevant>();
        var searchResult = await (await GetMemoryAsync(wiki)).SearchAsync(context, index: wiki.Id);
        if (searchResult.NoResult) return result;

        foreach (var item in searchResult.Results)
        {
            result.AddRange(item.Partitions.Select(part => new SourceRelevant
            {
                SourceName = item.SourceName,
                SourceUrl = item.SourceUrl,
                Text = part.Text,
                Relevance = part.Relevance,
                LastUpdate = part.LastUpdate
            }));
        }

        return result;
    }

    /// <summary>
    /// 获取知识库文档记录
    /// </summary>
    /// <param name="wiki"></param>
    /// <param name="documentId"></param>
    /// <returns></returns>
    public async Task<List<DocumentRecord>> MemoryRecordListAsync(AiWiki wiki, string? documentId)
    {
        var result = new List<DocumentRecord>();
        var memory = await GetMemoryAsync(wiki);
        var memoryDbs = memory.Orchestrator.GetMemoryDbs();

        var filters = new List<MemoryFilter>();
        if (!documentId.IsNullOrWhiteSpace())
        {
            filters.Add(new MemoryFilter().ByDocument(documentId!));
        }

        foreach (var memoryDb in memoryDbs)
        {
            var items = await memoryDb.GetListAsync(wiki.Id, filters, 100, true).ToListAsync();
            result.AddRange(items.Select(item => new DocumentRecord
            {
                DocumentId = item.GetDocumentId(),
                Text = item.GetPartitionText(),
                WebPageUrl = item.GetWebPageUrl(),
                LastUpdate = item.GetLastUpdate(),
                File = item.GetFileName()
            }));
        }

        return result;
    }
}