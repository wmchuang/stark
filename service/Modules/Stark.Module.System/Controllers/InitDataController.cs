using IPTools.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stark.Module.System.Domain;
using Stark.Module.System.Infrastructure;
using Stark.Starter.Core.Enum;
using Stark.Starter.Web.Hub;
using Volo.Abp;

namespace Stark.Module.System.Controllers;

/// <summary>
/// 数据初始化
/// </summary>
[AllowAnonymous]
public class InitDataController : BaseController
{
    private readonly SystemDbContext _context;
    private readonly INoticeService _noticeService;

    public InitDataController(SystemDbContext context, INoticeService noticeService)
    {
        _context = context;
        _noticeService = noticeService;
    }

    /// <summary>
    /// 初始化程序
    /// </summary>
    /// <exception cref="UserFriendlyException"></exception>
    [HttpGet]
    public async Task InitAsync()
    {
        if (await _context.SysMenu.AnyAsync())
            throw new UserFriendlyException("数据已初始化");

        var dept = new SySDept("史塔克科技", 0);

        var role = new SysRole(SystemConst.SuperAdmin, "superAdmin", 0, RoleDataScopeEnum.All, "超级管理员");

        var addMenu = new List<SysMenu>();
        var parentMenu = new SysMenu("系统设置", "0", 100, "", MenuTypeEnum.Catalog, "system", "Setting", false);

        var parentMenuA = new SysMenu("用户管理", parentMenu.Id, 1, "", MenuTypeEnum.Menu, "system_user", "UserFilled", false);
        var parentMenuB = new SysMenu("角色管理", parentMenu.Id, 2, "", MenuTypeEnum.Menu, "system_role", "CameraFilled", false);
        var parentMenuC = new SysMenu("菜单管理", parentMenu.Id, 3, "", MenuTypeEnum.Menu, "system_menu", "Menu", false);
        var parentMenuD = new SysMenu("部门管理", parentMenu.Id, 4, "", MenuTypeEnum.Menu, "system_dept", "Histogram", false);
        var parentMenuE = new SysMenu("访问日志", parentMenu.Id, 5, "", MenuTypeEnum.Menu, "system_logvisit", "Sunrise", false);
        var parentMenuF = new SysMenu("异常日志", parentMenu.Id, 6, "", MenuTypeEnum.Menu, "system_logex", "WarnTriangleFilled",
            false);
        addMenu.Add(parentMenu);
        addMenu.Add(parentMenuA);
        addMenu.Add(parentMenuB);
        addMenu.Add(parentMenuC);
        addMenu.Add(parentMenuD);
        addMenu.Add(parentMenuE);
        addMenu.Add(parentMenuF);
        var list = new List<SysMenu>
        {
            new SysMenu("查询", parentMenuA.Id, 1, "", MenuTypeEnum.Button, "system_user_query", "", false),
            new SysMenu("添加", parentMenuA.Id, 2, "", MenuTypeEnum.Button, "system_user_add", "", false),
            new SysMenu("编辑", parentMenuA.Id, 3, "", MenuTypeEnum.Button, "system_user_edit", "", false),
            new SysMenu("分配角色", parentMenuA.Id, 4, "", MenuTypeEnum.Button, "system_user_edit_role", "", false),
            new SysMenu("重置密码", parentMenuA.Id, 5, "", MenuTypeEnum.Button, "system_user_resetPwd", "", false),
            new SysMenu("删除", parentMenuA.Id, 6, "", MenuTypeEnum.Button, "system_user_delete", "", false),
        };
        addMenu.AddRange(list);

        list = new List<SysMenu>
        {
            new SysMenu("查询", parentMenuB.Id, 1, "", MenuTypeEnum.Button, "role_query", "", false),
            new SysMenu("添加", parentMenuB.Id, 2, "", MenuTypeEnum.Button, "role_add", "", false),
            new SysMenu("编辑", parentMenuB.Id, 3, "", MenuTypeEnum.Button, "role_edit", "", false),
            new SysMenu("删除", parentMenuB.Id, 4, "", MenuTypeEnum.Button, "role_delete", "", false),
        };
        addMenu.AddRange(list);

        list = new List<SysMenu>
        {
            new SysMenu("查询", parentMenuC.Id, 1, "", MenuTypeEnum.Button, "system_menu_query", "", false),
            new SysMenu("添加", parentMenuC.Id, 2, "", MenuTypeEnum.Button, "system_menu_add", "", false),
            new SysMenu("编辑", parentMenuC.Id, 3, "", MenuTypeEnum.Button, "system_menu_edit", "", false),
            new SysMenu("删除", parentMenuC.Id, 4, "", MenuTypeEnum.Button, "system_menu_delete", "", false),
        };

        addMenu.AddRange(list);

        list = new List<SysMenu>
        {
            new SysMenu("查询", parentMenuD.Id, 1, "", MenuTypeEnum.Button, "system_dept_query", "", false),
            new SysMenu("添加", parentMenuD.Id, 2, "", MenuTypeEnum.Button, "system_dept_add", "", false),
            new SysMenu("编辑", parentMenuD.Id, 3, "", MenuTypeEnum.Button, "system_dept_edit", "", false),
            new SysMenu("删除", parentMenuD.Id, 4, "", MenuTypeEnum.Button, "system_dept_delete", "", false),
        };

        addMenu.AddRange(list);

        list = new List<SysMenu>
        {
            new SysMenu("查询", parentMenuE.Id, 1, "", MenuTypeEnum.Button, "system_logvisit_query", "", false),
        };

        addMenu.AddRange(list);

        list = new List<SysMenu>
        {
            new SysMenu("查询", parentMenuF.Id, 1, "", MenuTypeEnum.Button, "system_logex_query", "", false),
        };
        addMenu.AddRange(list);

        var user = new SysUser();
        user.SetBaseInfo(dept.Id, "superadmin", "superadmin", "18111111111", "超级管理员");
        user.RestPassword();

        var userRole = new SysUserRole(user.Id, role.Id);

        await _context.SySDept.AddAsync(dept);
        await _context.SysRole.AddAsync(role);
        await _context.SysMenu.AddRangeAsync(addMenu);
        await _context.SysUser.AddAsync(user);
        await _context.SysUserRole.AddAsync(userRole);

        await _context.SaveChangesAsync();
    }

    [HttpGet]
    public async Task Test()
    {
        await _noticeService.SendToAllClientsAsync("量化成功");
    }
}