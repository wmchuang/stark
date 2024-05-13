using Stark.Module.AI.Models.Result;
using Microsoft.AspNetCore.Mvc;
using Stark.Module.AI.Application.Service;
using Stark.Module.AI.Domain;
using Stark.Module.AI.Models.Request;
using Stark.Starter.Web.Models;

namespace Stark.Module.AI.Controllers;

/// <summary>
/// 智能体
/// </summary>
[Route("api/ai/bot/[action]")]
public class AiBotController : BaseController
{
    private readonly AiBotService _service;

    public AiBotController(AiBotService service)
    {
        _service = service;
    }

    /// <summary>
    /// 添加智能体
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("add")]
    public Task AddBot(BotAddRequest request)
    {
        return _service.AddAsync(request);
    }
    
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("update")]
    public Task UpdateBot(BotUpdateRequest request)
    {
        return _service.UpdateAsync(request);
    }

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("page")]
    public Task<PageResult<AiBot>> PageAsync(PageRequest request)
    {
        return _service.PageAsync(request);
    }
    
    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    [HttpGet]
    [ActionName("detail")]
    public Task<AiBot> DetailAsync([FromQuery]string botId)
    {
        return _service.DetailAsync(botId);
    }
}