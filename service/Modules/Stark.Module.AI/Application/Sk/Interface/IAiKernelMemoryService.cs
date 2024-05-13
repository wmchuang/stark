using Microsoft.KernelMemory;
using Stark.Module.AI.Domain;
using Stark.Module.AI.Models.Dto;
using Stark.Module.AI.Models.Result;
using Volo.Abp.DependencyInjection;

namespace Stark.Module.AI.Application.Sk;

/// <summary>
/// 
/// </summary>
public interface IAiKernelMemoryService : ITransientDependency
{
    public Task<MemoryServerless> GetMemoryAsync(AiWiki aiWiki);

    /// <summary>
    /// 获取某文本在知识库中的文档相关度
    /// </summary>
    /// <param name="wiki"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<List<SourceRelevant>> SourceRelevantListAsync(AiWiki wiki, string context);

    /// <summary>
    ///  获取知识库文档记录
    /// </summary>
    /// <param name="wiki"></param>
    /// <param name="documentId"></param>
    /// <returns></returns>
    Task<List<DocumentRecord>> MemoryRecordListAsync(AiWiki wiki, string? documentId);
}