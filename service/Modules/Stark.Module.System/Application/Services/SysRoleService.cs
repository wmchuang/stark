using Microsoft.EntityFrameworkCore;
using SqlSugar;
using Stark.Module.System.Domain;
using Stark.Module.System.Infrastructure;
using Stark.Module.System.Models.Requests.SysRoles;
using Stark.Module.System.Models.Results.SysRoles;
using Stark.Starter.Core;
using Stark.Starter.Core.Const;
using Stark.Starter.Web.Models;
using Volo.Abp;

namespace Stark.Module.System.Application.Services;

public class SysRoleService : BaseService
{
    /// <summary>
    /// 添加或修改
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task AddOrEditAsync(AddRoleRequest dto)
    {
        var role = new SysRole();
        if (!string.IsNullOrWhiteSpace(dto.Id))
        {
            role = await _dbContext.SysRole.FirstOrDefaultAsync(sm => sm.Id == dto.Id);
            if (role == null)
                throw new UserFriendlyException("角色不存在");
            if (SystemConst.SuperAdmin.Equals(role.Name))
                throw new UserFriendlyException("不能修改超级管理员角色");
        }

        role.SetBaseInfo(dto.Name, dto.Key, dto.Sort, dto.Remark);
        role.Status = dto.Status;
        if (string.IsNullOrWhiteSpace(dto.Id))
        {
            await _dbContext.SysRole.AddAsync(role);
        }
        else
        {
            _dbContext.SysRole.Update(role);

            var roleMenus = await _dbContext.SysRoleMenu.Where(sm => sm.RoleId == dto.Id).ToListAsync();
            if (roleMenus.Any())
                _dbContext.SysRoleMenu.RemoveRange(roleMenus);
        }

        var srmList = dto.MenuIds.Select(m => new SysRoleMenu(role.Id, m));
        await _dbContext.SysRoleMenu.AddRangeAsync(srmList);

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 修改数据权限
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task UpdateDataScopeAsync(UpdateRoleDataScopeRequest dto)
    {
        var role = await _dbContext.SysRole.FirstOrDefaultAsync(sm => sm.Id == dto.RoleId);
        if (role == null)
            throw new UserFriendlyException("未找到当前角色信息");

        role.SetDataScope(dto.DataScope);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="roleId"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task DeleteAsync(string roleId)
    {
        var role = await _dbContext.SysRole.Where(x => x.Id == roleId).FirstAsync();
        if (SystemConst.SuperAdmin.Equals(role.Name))
            throw new UserFriendlyException("不能删除超级管理员角色");

        _dbContext.SysRole.Remove(role);
        await _dbContext.SaveChangesAsync();
    }
    
    /// <summary>
    /// 角色拥有的菜单
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<List<string>> GetHasMenuAsync(string roleId)
    {
        //查询没有下一级的菜单ID   举例：如果a有下一级，下一级有三个菜单数据（def），你只有d权限，
        //如果返回了a,前端接收到以后如果会直接根据有a的权限直接选中a下的所有，所以这个时候只返回d不返回a
        var query =   _baseQuery.Db.Queryable<SysMenu, SysRoleMenu>((m, rm) => new JoinQueryInfos(JoinType.Inner, m.Id == rm.MenuId && rm.RoleId == roleId));
        var parentIds = await query.Select((m, rm) => m.ParentId).ToListAsync();
        var menuIds = await query.Where((m, rm) => !parentIds.Contains(m.Id)).OrderBy((m, rm) => m.Sort).Select((m, rm) => m.Id).ToListAsync();

        return menuIds;
    }

    /// <summary>
    /// 角色分页列表
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<PageResult<SysRoleResult>> PageAsync(RolePageRequest dto)
    {
        var total = new RefAsync<int>(0);
        var role = await _baseQuery.Db.Queryable<SysRole>()
            .WhereIF(!dto.RoleName.IsNullOrWhiteSpace(), x => x.Name.Contains(dto.RoleName))
            .ToPageListAsync(dto.PageNo, dto.PageSize, total);

        var result = new PageResult<SysRoleResult>
        {
            TotalCount = total,
            Rows = _mapper.Map<List<SysRoleResult>>(role),
        };
        return result;
    }

    /// <summary>
    /// 列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<SysRoleListResult>> ListAsync()
    {
        return await _baseQuery.Db.Queryable<SysRole>()
            .Where(x => x.Status == StarkConst.StatusYes)
            .Select(x => new SysRoleListResult
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
    }
}