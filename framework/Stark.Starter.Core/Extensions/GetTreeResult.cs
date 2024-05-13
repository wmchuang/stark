namespace Stark.Starter.Core.Extensions;

public class GetTreeResult<T>
{
    public string Id { get; set; }
    
    public string ParentId { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    public List<T> Children { get; set; }
}