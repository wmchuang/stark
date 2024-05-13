using Microsoft.EntityFrameworkCore;
using SqlSugar;
using Stark.Module.System.Domain;
using Stark.Module.System.Infrastructure;
using Stark.Module.System.Models.Requests.Auth;
using Stark.Module.System.Models.Results.Auth;
using Stark.Starter.Core;
using Stark.Starter.Core.Const;
using Stark.Starter.Core.Enum;
using Stark.Starter.Web.Jwt;
using Volo.Abp;

namespace Stark.Module.System.Application.Services;

public class AuthService : BaseService
{
    private readonly TokenService _tokenService;

    public AuthService(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task<string> LoginAsync(LoginRequest dto)
    {
        var user = await _dbContext.SysUser.Where(x => x.LoginName == dto.UserName).FirstOrDefaultAsync();
        if (user == null)
            throw new UserFriendlyException("账户不存在");

        if (user.Status == StarkConst.StatusNo)
            throw new UserFriendlyException("该账户已不可用");

        if (!user.VerityPassword(dto.Password))
            throw new UserFriendlyException("用户账号密码错误");

        return await _tokenService.CreateTokenAsync(user.Id, user.UserName, user.LoginName);
    }

    public virtual async Task<UserRouterResult> GetRouteAsync()
    {
        var userId = _operator.GetOperator().OperatorId;
        var isSuperAdmin = await GetIsSuperAdmin(userId);

        List<SysMenu> menuList;
        var tmpMenuTypeArray = new[] { MenuTypeEnum.Catalog, MenuTypeEnum.Menu };
        if (isSuperAdmin)
        {
            menuList = await _baseQuery.Db.Queryable<SysMenu>().OrderBy(x => x.Sort).ToListAsync();
        }
        else
        {
            var roleIds = await _baseQuery.Db.Queryable<SysRole, SysUserRole>((r, ur) => new JoinQueryInfos(
                    JoinType.Inner, r.Id == ur.RoleId && ur.UserId == userId))
                .Where(r => r.Status == StarkConst.StatusYes)
                .Select(r => r.Id)
                .ToListAsync();
            if (!roleIds.Any())
                return new UserRouterResult();

            menuList = await _baseQuery.Db.Queryable<SysMenu, SysRoleMenu>((m, rm) => new JoinQueryInfos(
                    JoinType.Inner, m.Id == rm.MenuId && roleIds.Contains(rm.RoleId)))
                .OrderBy(m => m.Sort)
                .ToListAsync();
        }

        //查询不包含按钮的信息
        var notButtonList = menuList.Where(t => tmpMenuTypeArray.Contains(t.Type)).ToList();
        var buttonCodeList = menuList.Where(t => t.Type == MenuTypeEnum.Button).Select(t => t.Code).ToList();

        return new UserRouterResult()
        {
            RouterResults = notButtonList.Select(x => new RouterResult().Build(x)).ToList(),
            Buttons = buttonCodeList
        };
    }

    /// <summary>
    /// 获取用户角色是否是超级管理员
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private async Task<bool> GetIsSuperAdmin(string userId)
    {
        return await _baseQuery.Db.Queryable<SysRole, SysUserRole>((r, ur) => new JoinQueryInfos(
                JoinType.Inner, r.Id == ur.RoleId && ur.UserId == userId))
            .Where(r => r.Name == SystemConst.SuperAdmin).AnyAsync();
    }
}