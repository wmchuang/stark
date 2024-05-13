using Microsoft.AspNetCore.Mvc;
using Stark.Module.AI.Application.Service;
using Stark.Module.AI.Domain;
using Stark.Module.AI.Models.Request;
using Stark.Starter.Web.Models;

namespace Stark.Module.AI.Controllers;

/// <summary>
/// 知识库文档
/// </summary>
[Route("api/ai/wiki/document/[action]")]
public class AiWikiDocumentController : BaseController
{
    private readonly AiWikiDocumentService _documentService;

    public AiWikiDocumentController(AiWikiDocumentService documentService)
    {
        _documentService = documentService;
    }

    /// <summary>
    /// 添加文档
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("add")]
    public Task AddWikiDocument([FromBody] WikiDocumentAddRequest request)
    {
        return _documentService.AddWikiDocument(request);
    }

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("page")]
    public Task<PageResult<AiWikiDocument>> PageAsync(WikiDocumentPageRequest request)
    {
        return _documentService.PageAsync(request);
    }
}