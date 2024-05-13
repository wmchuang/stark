using Microsoft.AspNetCore.Mvc;
using Stark.Module.System.Application.Services;
using Stark.Module.System.Domain;
using Stark.Module.System.Models.Requests.SysMenu;
using Stark.Module.System.Models.Results.SysMenu;

namespace Stark.Module.System.Controllers;

/// <summary>
/// 菜单
/// </summary>
public class SysMenuController : BaseController
{
    private readonly SysMenuService _sysMenuService;

    public SysMenuController(SysMenuService sysMenuService)
    {
        _sysMenuService = sysMenuService;
    }

    /// <summary>
    /// 添加菜单
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public Task AddAsync(AddMenuRequest dto)
    {
        return _sysMenuService.AddAsync(dto);
    }

    /// <summary>
    /// 修改菜单
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public Task UpdateAsync(UpdateMenuRequest dto)
    {
        return _sysMenuService.UpdateAsync(dto);
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    [HttpPost("{menuId}")]
    public Task DeleteAsync(string menuId)
    {
        return _sysMenuService.DeleteAsync(menuId);
    }

    /// <summary>
    /// 获取菜单树结构列表
    /// </summary>
    [HttpPost]
    public async Task<List<SysMenuTreeListResult>> GetTreeAsync(MenuListRequest dto)
    {
        return await _sysMenuService.GetTreeAsync(dto);
    }
    
    /// <summary>
    /// 获取菜单
    /// </summary>
    [HttpGet("{menuId}")]
    public async Task<SysMenu> GetAsync(string menuId)
    {
        return await _sysMenuService.GetAsync(menuId);
    }
}