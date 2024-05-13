namespace Stark.Starter.Web.Models;

public class PageResult<T>
{
    /// <summary>
    /// 总记录数
    /// </summary>
    public long TotalCount { get; set; } = 0;

    /// <summary>
    /// 数据
    /// </summary>
    public List<T> Rows { get; set; } = new();
}