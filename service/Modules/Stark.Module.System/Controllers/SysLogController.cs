using Microsoft.AspNetCore.Mvc;
using Stark.Module.System.Application.Services;
using Stark.Module.System.Domain;
using Stark.Module.System.Models.Requests.SysLogVisit;
using Stark.Starter.Web.Models;

namespace Stark.Module.System.Controllers;

/// <summary>
/// 日志服务
/// </summary>
public class SysLogController : BaseController
{
    private readonly SysLogVisitService _sysLogVisitService;
    private readonly SysLogExService _sysLogExService;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="sysLogVisitService"></param>
    /// <param name="sysLogExService"></param>
    public SysLogController(SysLogVisitService sysLogVisitService,SysLogExService sysLogExService)
    {
        _sysLogVisitService = sysLogVisitService;
        _sysLogExService = sysLogExService;
    }

    /// <summary>
    /// 访问日志分页列表
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResult<SysLogVisit>> VisitPageAsync(LogVisitPageRequest dto)
    {
        return await _sysLogVisitService.PageAsync(dto);
    }
    
    /// <summary>
    /// 异常日志分页列表
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResult<SysLogEx>> ExPageAsync(LogVisitPageRequest dto)
    {
        return await _sysLogExService.PageAsync(dto);
    }
}