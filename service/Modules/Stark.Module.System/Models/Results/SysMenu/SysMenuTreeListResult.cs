using Stark.Starter.Core.Enum;
using Stark.Starter.Core.Extensions;

namespace Stark.Module.System.Models.Results.SysMenu;

public class SysMenuTreeListResult : GetTreeResult<SysMenuTreeListResult>
{
    /// <summary>
    /// 菜单名称
    /// </summary>
    public string MenuName { get; set; } = string.Empty;
    
    /// <summary>
    /// 菜单类型；0：目录，1：菜单，2：按钮
    /// </summary>
    public MenuTypeEnum MenuType { get; set; }
    
    /// <summary>
    ///  显示false  隐藏true
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// 菜单标识
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 菜单图标
    /// </summary>
    public string Icon { get; set; } = string.Empty;


}