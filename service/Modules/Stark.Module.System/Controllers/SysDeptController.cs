using Microsoft.AspNetCore.Mvc;
using Stark.Module.System.Application.Services;
using Stark.Module.System.Models.Requests.SysDept;
using Stark.Module.System.Models.Results.SysDept;

namespace Stark.Module.System.Controllers;

/// <summary>
/// 部门
/// </summary>
public class SysDeptController : BaseController
{
    private readonly SySDeptService _sySDeptService;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="sySDeptService"></param>
    public SysDeptController(SySDeptService sySDeptService)
    {
        _sySDeptService = sySDeptService;
    }

    /// <summary>
    /// 添加部门
    /// </summary>
    /// <param name="dto"></param>
    [HttpPost]
    public async Task AddAsync(AddSysDeptRequest dto)
    {
        await _sySDeptService.AddAsync(dto);
    }

    /// <summary>
    /// 修改部门
    /// </summary>
    /// <param name="dto"></param>
    [HttpPost]
    public async Task UpdateAsync(UpdateSysDeptRequest dto)
    {
        await _sySDeptService.UpdateAsync(dto);
    }
    
    /// <summary>
    /// 删除部门
    /// </summary>
    /// <param name="deptId"></param>
    [HttpPost("{deptId}")]
    public async Task DeleteAsync(string deptId)
    {
        await _sySDeptService.DeleteAsync(deptId);
    }

    /// <summary>
    /// 获取部门树结构列表
    /// </summary>
    /// <param name="dto"></param>
    [HttpPost]
    public async Task<List<SysDeptTreeListResult>> GetTreeAsync(SysDeptListRequest dto)
    {
        return await _sySDeptService.GetTreeAsync(dto);
    }
}