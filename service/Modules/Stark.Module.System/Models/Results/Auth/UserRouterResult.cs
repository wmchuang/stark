namespace Stark.Module.System.Models.Results.Auth;

public class UserRouterResult
{
    /// <summary>
    /// 路由
    /// </summary>
    public List<RouterResult> RouterResults { get; set; } = new List<RouterResult>();

    /// <summary>
    /// 按钮列表
    /// </summary>
    public List<string> Buttons { get; set; } = new List<string>();
}