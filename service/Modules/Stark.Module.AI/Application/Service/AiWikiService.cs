using Microsoft.EntityFrameworkCore;
using Microsoft.KernelMemory.MemoryStorage;
using SqlSugar;
using Stark.Module.AI.Application.Sk;
using Stark.Module.AI.Domain;
using Stark.Module.AI.Models.Dto;
using Stark.Module.AI.Models.Request;
using Stark.Module.AI.Models.Result;
using Stark.Starter.Web.Models;
using SystemModule.Application;

namespace Stark.Module.AI.Application.Service;

/// <summary>
/// 知识库服务
/// </summary>
public class AiWikiService : BaseService
{
    private readonly IAiKernelMemoryService _aiKernelMemoryService;

    public AiWikiService(IAiKernelMemoryService aiKernelMemoryService)
    {
        _aiKernelMemoryService = aiKernelMemoryService;
    }

    /// <summary>
    /// 添加知识库
    /// </summary>
    /// <param name="request"></param>
    public async Task AddWikiAsync(WikiAddRequest request)
    {
        await _dbContext.AiWiki.AddAsync(new AiWiki()
        {
            WikiName = request.WikiName,
            Description = request.Description,
            ChatModelId = request.ChatModelId,
            EmbeddingModelId = request.EmbeddingModelId,
            DbType = request.DbType,
            ConnectionString = request.ConnectionString
        });
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 修改知识库
    /// </summary>
    /// <param name="request"></param>
    public async Task UpdateWikiAsync(WikiUpdateRequest request)
    {
        var entity = await _dbContext.AiWiki.FirstAsync(x => x.Id == request.Id);
        entity.WikiName = request.WikiName;
        entity.Description = request.Description;
        entity.ChatModelId = request.ChatModelId;
        entity.EmbeddingModelId = request.EmbeddingModelId;
        entity.DbType = request.DbType;
        entity.ConnectionString = request.ConnectionString;

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 知识库分页列表
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<PageResult<AiWiki>> PageAsync(PageRequest dto)
    {
        var total = new RefAsync<int>(0);
        var listEntity = await _baseQuery.Db.Queryable<AiWiki>()
            .ToPageListAsync(dto.PageNo, dto.PageSize, total);

        var result = new PageResult<AiWiki>
        {
            TotalCount = total,
            Rows = listEntity
        };
        return result;
    }

    /// <summary>
    /// 知识库列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<AiWiki>> ListAsync()
    {
        return await _baseQuery.Db.Queryable<AiWiki>()
            .ToListAsync();
    }

    /// <summary>
    /// 知识库
    /// </summary>
    /// <returns></returns>
    public async Task<AiWiki> GetAsync(string wikiId)
    {
        return await _baseQuery.Db.Queryable<AiWiki>().FirstAsync(x => x.Id == wikiId);
    }

    /// <summary>
    /// 获取某文本在知识库中的文档相关度
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<List<SourceRelevant>> SourceRelevantListAsync(GetSourceRelevantRequest request)
    {
        var wiki = await GetAsync(request.WikiId);
        return await _aiKernelMemoryService.SourceRelevantListAsync(wiki, request.Context);
    }

    /// <summary>
    /// 获取知识库文档记录
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<List<DocumentRecord>> MemoryRecordListAsync(GetMemoryRecordRequest request)
    {
        var wiki = await GetAsync(request.WikiId);
        return await _aiKernelMemoryService.MemoryRecordListAsync(wiki, request.DocumentId);
    }
}