using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Stark.Module.System.Domain;
using Stark.Module.System.Models.Requests.SysMenu;
using Stark.Module.System.Models.Results.SysMenu;
using Stark.Starter.Core.Enum;
using Stark.Starter.Core.Extensions;
using Volo.Abp;

namespace Stark.Module.System.Application.Services;

/// <summary>
/// 菜单
/// </summary>
public class SysMenuService : BaseService
{
    /// <summary>
    /// 添加菜单
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task AddAsync(AddMenuRequest dto)
    {
        await InputVerifyAndGetRootPath(dto);
        var menus = new List<SysMenu>();

        var menu = new SysMenu(dto.MenuName, dto.ParentId, dto.Sort, dto.Link, dto.MenuType,
            dto.Code, dto.Icon, dto.Hidden);
        menus.Add(menu);
        //默认为菜单添加一个查询按钮
        if (menu.Type == MenuTypeEnum.Menu)
        {
            menus.Add(new SysMenu(menu.Code, menu.Id));
        }

        await _dbContext.SysMenu.AddRangeAsync(menus);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 修改菜单
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task UpdateAsync(UpdateMenuRequest dto)
    {
        var menu = await _dbContext.SysMenu.FirstOrDefaultAsync(sm => sm.Id == dto.MenuId);
        if (menu == default)
            throw new UserFriendlyException("菜单标识无效");

        if (menu.Type == MenuTypeEnum.Menu && dto.MenuType == MenuTypeEnum.Catalog)
        {
            //菜单升级为目录  下面有按钮 不能升级
            var existButton = await ExistMenu(t => t.ParentId == menu.Id && t.Type == MenuTypeEnum.Button);
            if (existButton)
                throw new UserFriendlyException("该目录下存在菜单，故不能升级为目录");
        }
        else if (menu.Type == MenuTypeEnum.Catalog && dto.MenuType == MenuTypeEnum.Menu)
        {
            //目录降级为菜单 如果下面有菜单 就不能降级
            var existMenu = await ExistMenu(t => t.ParentId == menu.Id && t.Type == MenuTypeEnum.Menu);
            if (existMenu)
                throw new UserFriendlyException("该目录下存在菜单，故不能降级为目录");
        }

        await InputVerifyAndGetRootPath(dto);

        menu.SetBaseInfo(dto.MenuName, dto.ParentId, dto.Sort, dto.Link,
            dto.MenuType, dto.Code, dto.Icon, dto.Hidden);
        //如果不存在查询按钮，那么添加一个查询按钮
        if (dto.MenuType == MenuTypeEnum.Menu)
        {
            var queryMenuCode = menu.Code + "_query";
            var existQueryButton = await _dbContext.SysMenu.AnyAsync(t => t.ParentId == menu.Id && t.Code == queryMenuCode);
            if (!existQueryButton)
                await _dbContext.SysMenu.AddAsync(new SysMenu(menu.Code, menu.Id));
        }

        _dbContext.SysMenu.Update(menu);

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="menuId"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task DeleteAsync(string menuId)
    {
        var menu = await _dbContext.SysMenu.FirstOrDefaultAsync(x => x.Id == menuId);
        if (menu == null)
            throw new UserFriendlyException("未查询到菜单信息");

        //菜单有下级的话先让删除下级再让删除该菜单
        var existMenuChild = await _dbContext.SysMenu.AnyAsync(x => x.ParentId == menuId);
        if (existMenuChild)
            throw new UserFriendlyException("存在下级，请先删除下级菜单");

        //菜单表逻辑删除
        _dbContext.SysMenu.Remove(menu);

        //角色菜单表物理删除
        var roleMenu = await _dbContext.SysRoleMenu.Where(t => t.MenuId == menuId).ToListAsync();
        _dbContext.SysRoleMenu.RemoveRange(roleMenu);

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 判断是否存在菜单
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    private async Task<bool> ExistMenu(Expression<Func<SysMenu, bool>> expression)
    {
        return await _dbContext.SysMenu.AnyAsync(expression);
    }

    /// <summary>
    /// 校验父id返回根目录
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    private async Task InputVerifyAndGetRootPath(AddMenuRequest dto)
    {
        //目录下只允许添加目录或菜单”、“菜单下只允许添加按钮”、“按钮下不可添加目录或菜单”
        if (dto.ParentId == "0")
        {
            if (dto.MenuType == MenuTypeEnum.Button)
                throw new UserFriendlyException("目录下只允许添加目录或菜单");
        }
        else
        {
            var parentIdMenuEntity = await _dbContext.SysMenu.FirstAsync(t => t.Id == dto.ParentId);
            if (parentIdMenuEntity == default)
                throw new UserFriendlyException("上级菜单标识无效");
            if (parentIdMenuEntity.Type == MenuTypeEnum.Catalog && dto.MenuType == MenuTypeEnum.Button)
                throw new UserFriendlyException("目录下只允许添加目录或菜单");
            if (parentIdMenuEntity.Type == MenuTypeEnum.Menu && dto.MenuType != MenuTypeEnum.Button)
                throw new UserFriendlyException("菜单下只允许添加按钮");
            if (parentIdMenuEntity.Type == MenuTypeEnum.Button)
                throw new UserFriendlyException("按钮下不可添加目录、菜单、按钮");
        }
    }

    /// <summary>
    /// 菜单树
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<List<SysMenuTreeListResult>> GetTreeAsync(MenuListRequest dto)
    {
        var menuIdList = await _baseQuery.Db.Queryable<SysMenu>()
            .WhereIF(!dto.MenuName.IsNullOrWhiteSpace(), x => x.Name.Contains(dto.MenuName))
            .Select(x => new SysMenuTreeListResult
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Sort = x.Sort,
                MenuName = x.Name,
                MenuType = x.Type,
                Hidden = x.Hidden,
                Code = x.Code,
                Icon = x.Icon
            }).ToListAsync();

        return TreeSelectExtension.HandleTreeChildren(menuIdList);
    }

    /// <summary>
    /// 获取详情
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    public async Task<SysMenu> GetAsync(string menuId)
    {
        return await _baseQuery.Db.Queryable<SysMenu>()
            .Where(x => x.Id == menuId)
            .FirstAsync();
    }
}