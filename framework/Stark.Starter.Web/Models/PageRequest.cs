namespace Stark.Starter.Web.Models;

public class PageRequest
{
    /// <summary>
    /// 当前页
    /// </summary>
    public int PageNo { get; set; } = 1;

    /// <summary>
    /// 每页个数
    /// </summary>
    public int PageSize { get; set; } = 10;
}