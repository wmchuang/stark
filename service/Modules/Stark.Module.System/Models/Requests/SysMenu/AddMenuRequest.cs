using System.ComponentModel.DataAnnotations;
using Stark.Starter.Core.Enum;

namespace Stark.Module.System.Models.Requests.SysMenu;

public class AddMenuRequest
{
    /// <summary>
    /// 菜单名称
    /// </summary>
    public string MenuName { get; set; }

    /// <summary>
    /// 父菜单ID
    /// </summary>
    public string ParentId { get; set; }

    /// <summary>
    /// 显示顺序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否是外链
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// 菜单类型；0：目录，1：菜单，2：按钮	
    /// </summary>
    public MenuTypeEnum MenuType { get; set; } 

    /// <summary>
    /// 显示状态
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// 显菜单状态
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 菜单编号
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 菜单图标
    /// </summary>
    public string Icon { get; set; }
}

/// <summary>
/// 修改菜单
/// </summary>
public class UpdateMenuRequest : AddMenuRequest
{
    /// <summary>
    /// 菜单Id
    /// </summary>
    [Required]
    public string MenuId { get; set; }
}