using Microsoft.AspNetCore.Mvc;
using Stark.Module.System.Application.Services;
using Stark.Module.System.Models.Requests.SysRoles;
using Stark.Module.System.Models.Results.SysRoles;
using Stark.Starter.Web.Models;

namespace Stark.Module.System.Controllers;

/// <summary>
/// 角色
/// </summary>
public class SysRoleController : BaseController
{
    private readonly SysRoleService _sysRoleService;

    public SysRoleController(SysRoleService sysRoleService)
    {
        _sysRoleService = sysRoleService;
    }

    /// <summary>
    /// 添加或修改
    /// </summary>
    /// <param name="dto"></param>
    [HttpPost]
    public async Task AddOrEditAsync(AddRoleRequest dto)
    {
        await _sysRoleService.AddOrEditAsync(dto);
    }

    /// <summary>
    /// 修改数据权限
    /// </summary>
    /// <param name="dto"></param>
    [HttpPost]
    public async Task UpdateDataScopeAsync(UpdateRoleDataScopeRequest dto)
    {
        await _sysRoleService.UpdateDataScopeAsync(dto);
    }
    
    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="roleId"></param>
    [HttpPost("{roleId}")]
    public async Task DeleteAsync(string roleId)
    {
        await _sysRoleService.DeleteAsync(roleId);
    }

    /// <summary>
    /// 角色分页列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResult<SysRoleResult>> PageAsync(RolePageRequest dto)
    {
        return await _sysRoleService.PageAsync(dto);
    }
    
    /// <summary>
    /// 角色列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<SysRoleListResult>> ListAsync()
    {
        return await _sysRoleService.ListAsync();
    }
    
    /// <summary>
    /// 获取角色拥有的菜单标识
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    [HttpGet("{roleId}")]
    public async Task<List<string>> GetHasMenuAsync(string roleId)
    {
        return await _sysRoleService.GetHasMenuAsync(roleId);
    }
}