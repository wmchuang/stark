using Microsoft.AspNetCore.Mvc;
using Microsoft.KernelMemory.MemoryStorage;
using Stark.Module.AI.Application.Service;
using Stark.Module.AI.Domain;
using Stark.Module.AI.Models.Dto;
using Stark.Module.AI.Models.Request;
using Stark.Module.AI.Models.Result;
using Stark.Starter.Web.Models;

namespace Stark.Module.AI.Controllers;

/// <summary>
/// 知识库
/// </summary>
[Route("api/ai/wiki/[action]")]
public class AiWikiController : BaseController
{
    private readonly AiWikiService _service;

    public AiWikiController(AiWikiService service)
    {
        _service = service;
    }

    /// <summary>
    /// 添加知识库
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("add")]
    public Task AddWiki([FromBody] WikiAddRequest request)
    {
        return _service.AddWikiAsync(request);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("update")]
    public Task UpdateWikiAsync(WikiUpdateRequest request)
    {
        return _service.UpdateWikiAsync(request);
    }

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("page")]
    public Task<PageResult<AiWiki>> PageAsync(PageRequest request)
    {
        return _service.PageAsync(request);
    }

    /// <summary>
    /// 列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ActionName("list")]
    public Task<List<AiWiki>> ListAsync()
    {
        return _service.ListAsync();
    }

    /// <summary>
    /// 获取某文本的相关文档相关度
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("relevant")]
    public async Task<List<SourceRelevant>> GetSourceRelevantListAsync(GetSourceRelevantRequest request)
    {
        return await _service.SourceRelevantListAsync(request);
    }

    /// <summary>
    /// 获取知识库文档记录
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ActionName("record")]
    public Task<List<DocumentRecord>> GetMemoryRecordListAsync(GetMemoryRecordRequest request)
    {
        return _service.MemoryRecordListAsync(request);
    }
}