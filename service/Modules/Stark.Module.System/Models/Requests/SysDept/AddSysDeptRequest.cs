using System.ComponentModel.DataAnnotations;

namespace Stark.Module.System.Models.Requests.SysDept;

/// <summary>
/// 添加部门
/// </summary>
public class AddSysDeptRequest
{
    /// <summary>
    /// 部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 父级部门Id
    /// </summary>
    public string? ParentId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}

/// <summary>
/// 修改部门
/// </summary>
public class UpdateSysDeptRequest : AddSysDeptRequest
{
    /// <summary>
    /// 部门Id
    /// </summary>
    [Required]
    public string Id { get; set; }
}