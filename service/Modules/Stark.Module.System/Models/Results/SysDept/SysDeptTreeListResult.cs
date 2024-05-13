using Stark.Starter.Core.Extensions;

namespace Stark.Module.System.Models.Results.SysDept;

public class SysDeptTreeListResult : GetTreeResult<SysDeptTreeListResult>
{
    /// <summary>
    /// 部门名称
    /// </summary>
    public string DeptName { get; set; }

    /// <summary>
    /// 创建人名称
    /// </summary>
    public string CreateName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}