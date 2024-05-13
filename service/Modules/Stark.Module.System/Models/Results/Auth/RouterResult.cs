using Stark.Starter.Core.Enum;

namespace Stark.Module.System.Models.Results.Auth;

public class RouterResult
{
    /// <summary>
    /// 菜单Id
    /// </summary>
    public string MenuId { get; set; }
    
    /// <summary>
    /// 菜单名称
    /// </summary>
    public string MenuName { get; set; }
    
    /// <summary>
    /// 父级Id
    /// </summary>
    public string ParentId { get; set; }
    
    public MenuTypeEnum MenuType { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public bool IsHide { get; set; }
    public string IsLink { get; set; }
    public string IsKeepAlive { get; set; }
    public string IsFull { get; set; }
    public string IsAffix { get; set; }
    public string Redirect { get; set; }
    
    
    public RouterResult Build(Domain.SysMenu sysMenu)
    {
        MenuId = sysMenu.Id;
        MenuName = sysMenu.Name;
        ParentId = sysMenu.ParentId;
        MenuType = sysMenu.Type;
        Code = sysMenu.Code;
        Name = sysMenu.Name;
        Icon = sysMenu.Icon;
        IsHide = sysMenu.Hidden;
        IsLink = sysMenu.Link;
        IsKeepAlive = "0";
        IsFull = "1";
        IsAffix = "1";
        Redirect = string.Empty;
        
        return this;
    }
}