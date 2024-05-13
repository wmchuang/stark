using Microsoft.AspNetCore.Mvc;
using Stark.Module.System.Application.Services;
using Stark.Module.System.Models.Requests.SysUser;
using Stark.Module.System.Models.Results.SysUser;
using Stark.Starter.DDD.Infrastructure.Operator;
using Stark.Starter.Web.Models;

namespace Stark.Module.System.Controllers;

/// <summary>
/// 用户
/// </summary>
public class SysUserController : BaseController
{
    private readonly SysUserService _sysUserService;
    private readonly IOperatorProvider _operatorProvider;

    public SysUserController(SysUserService sysUserService, IOperatorProvider operatorProvider)
    {
        _sysUserService = sysUserService;
        _operatorProvider = operatorProvider;
    }

    /// <summary>
    /// 添加或修改
    /// </summary>
    /// <param name="dto"></param>
    [HttpPost]
    public async Task AddOrEditAsync(AddOrEditUserRequest dto)
    {
        await _sysUserService.AddOrEditAsync(dto);
    }

    /// <summary>
    /// 设置角色
    /// </summary>
    /// <param name="dto"></param>
    [HttpPost]
    public async Task EditRoleAsync(EditUserRoleRequest dto)
    {
        await _sysUserService.EditRoleAsync(dto);
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="userId"></param>
    [HttpDelete("{userId}")]
    public async Task DeleteAsync(string userId)
    {
        await _sysUserService.DeleteAsync(userId);
    }

    /// <summary>
    /// 设置状态
    /// </summary>
    /// <param name="dto"></param>
    [HttpPost]
    public async Task EditStatusAsync(EditUserStatusRequest dto)
    {
        await _sysUserService.EditStatusAsync(dto);
    }

    /// <summary>
    /// 设置密码
    /// </summary>
    /// <param name="dto"></param>
    [HttpPost]
    public async Task EditPassword(EditUserPasswordRequest dto)
    {
        await _sysUserService.EditPassword(dto);
    }

    /// <summary>
    /// 设置密码
    /// </summary>
    /// <param name="dto"></param>
    [HttpPost]
    public async Task EditMyPassword(EditMyPasswordRequest dto)
    {
        await _sysUserService.EditMyPassword(dto);
    }
    
    /// <summary>
    /// 设置基本资料
    /// </summary>
    /// <param name="dto"></param>
    [HttpPost]
    public async Task EditMyInfo(EditUserInfoRequest dto)
    {
        await _sysUserService.EditInfo(dto);
    }

    /// <summary>
    /// 用户分页列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResult<UserPageResult>> PageAsync(UserPageRequest dto)
    {
        return await _sysUserService.PageAsync(dto);
    }

    /// <summary>
    /// 获取用户拥有的角色
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    public async Task<List<string>> GetHasRoleAsync(string userId)
    {
        return await _sysUserService.HasRoleAsync(userId);
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]  
    [HttpGet("{userId}")] 
    public async Task<UserInfoResult> GetUserAsync(string? userId)
    {
        if(userId.IsNullOrWhiteSpace())
            userId = _operatorProvider.GetOperator().OperatorId;
        
        return await _sysUserService.GetUserAsync(userId);
    }
}