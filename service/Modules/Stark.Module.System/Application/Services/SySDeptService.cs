using Microsoft.EntityFrameworkCore;
using Stark.Module.System.Domain;
using Stark.Module.System.Models.Requests.SysDept;
using Stark.Module.System.Models.Results.SysDept;
using Stark.Starter.Core;
using Stark.Starter.Core.Const;
using Stark.Starter.Core.Extensions;
using Volo.Abp;

namespace Stark.Module.System.Application.Services;

/// <summary>
/// 部门服务
/// </summary>
public class SySDeptService : BaseService
{
    /// <summary>
    /// 添加部门
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task AddAsync(AddSysDeptRequest dto)
    {
        var dept = new SySDept(dto.DeptName, dto.Sort);

        await SetDeptParent(dto, dept);

        await _dbContext.SySDept.AddAsync(dept);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 修改部门
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task UpdateAsync(UpdateSysDeptRequest dto)
    {
        var dept = await _dbContext.SySDept.Where(x => x.Id == dto.Id).FirstAsync();
        dept.DeptName = dto.DeptName;
        dept.Sort = dto.Sort;

        await SetDeptParent(dto, dept);

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 删除部门
    /// </summary>
    /// <param name="deptId"></param>
    public async Task DeleteAsync(string deptId)
    {
        var dept = await _dbContext.SySDept.Where(x => x.Id == deptId).FirstAsync();
        _dbContext.SySDept.Remove(dept);
        await _dbContext.SaveChangesAsync();
    }

    private async Task SetDeptParent(AddSysDeptRequest dto, SySDept dept)
    {
        if (!StarkConst.TreeRoot.Equals(dto.ParentId))
        {
            var parentDept = await _dbContext.SySDept.Where(x => x.Id == dto.ParentId).FirstOrDefaultAsync();
            if (parentDept == null)
                throw new UserFriendlyException("父级部门不存在");

            dept.SetTreePath(parentDept);
            
            //修改改部门下的所有部门的父级路径节点
            var childDeptList = await _dbContext.SySDept.Where(x => x.TreePath.Contains(dept.Id)).ToListAsync();
            if (childDeptList.Any())
            {
                childDeptList.Insert(0, dept);
                foreach (var sySDept in childDeptList.OrderBy(x => x.GetLevel()))
                {
                    if (sySDept.Id != dept.Id)
                    {
                        sySDept.SetTreePath(childDeptList.First(x => x.Id == sySDept.ParentDeptId));
                    }
                }
            }
        }
        //要把此部门从有父级修改成没有父级
        else
        {
            dept.SetEmptyParent();
        }
    }

    /// <summary>
    /// 获取树状数据
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<List<SysDeptTreeListResult>> GetTreeAsync(SysDeptListRequest dto)
    {
        var deptList = await _baseQuery.Db.Queryable<SySDept>()
            .WhereIF(!dto.DeptName.IsNullOrWhiteSpace(), x => x.DeptName.Contains(dto.DeptName))
            .Select(x => new SysDeptTreeListResult
            {
                Id = x.Id,
                ParentId = x.ParentDeptId,
                Sort = x.Sort,
                DeptName = x.DeptName,
                CreateName = x.CreateName,
                CreateTime = x.CreateTime
            }).ToListAsync();

        return TreeSelectExtension.HandleTreeChildren(deptList);
    }
}