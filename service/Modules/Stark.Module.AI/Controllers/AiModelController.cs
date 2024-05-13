using Microsoft.AspNetCore.Mvc;
using Stark.Module.AI.Application.Service;
using Stark.Module.AI.Domain;
using Stark.Module.AI.Models.Enum;
using Stark.Module.AI.Models.Request;
using Stark.Starter.Web.Models;

namespace Stark.Module.AI.Controllers;

/// <summary>
/// 模型管理
/// </summary>
[Route("api/ai/model/[action]")]
public class AiModelController : BaseController
{
    private readonly AiModelService _service;

    public AiModelController(AiModelService service)
    {
        _service = service;
    }

    /// <summary>
    /// 添加模型
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("add")]
    public Task AddModelAsync([FromBody] ModelAddRequest request)
    {
        return _service.AddModelAsync(request);
    }

    /// <summary>
    /// 复制模型
    /// </summary>
    /// <param name="modelId"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("copy")]
    public Task CopyModelAsync([FromQuery] string modelId)
    {
        return _service.CopyModelAsync(modelId);
    }

    /// <summary>
    /// 修改模型
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("update")]
    public Task UpdateModelAsync([FromBody] ModelUpdateRequest request)
    {
        return _service.UpdateModelAsync(request);
    }

    /// <summary>
    /// 模型分页数据
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("page")]
    public Task<PageResult<AiModel>> PageAsync([FromBody] ModelPageRequest request)
    {
        return _service.PageAsync(request);
    }

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="modelType"></param>
    /// <returns></returns>
    [HttpGet]
    [ActionName("list")]
    public async Task<List<AiModel>> ListAsync([FromQuery] AiModelTypeEnum? modelType)
    {
        return await _service.ListAsync(modelType);
    }
}