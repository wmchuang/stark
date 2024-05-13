using Microsoft.EntityFrameworkCore;
using SqlSugar;
using Stark.Module.System.Domain;
using Stark.Module.System.Models.Requests.SysUser;
using Stark.Module.System.Models.Results.SysUser;
using Stark.Starter.Core;
using Stark.Starter.Core.Const;
using Stark.Starter.DDD.Infrastructure.Operator;
using Stark.Starter.Web.Models;
using Volo.Abp;

namespace Stark.Module.System.Application.Services;

/// <summary>
/// 用户服务
/// </summary>
public class SysUserService : BaseService
{
    private readonly IOperatorProvider _operatorProvider;

    public SysUserService(IOperatorProvider operatorProvider)
    {
        _operatorProvider = operatorProvider;
    }

    /// <summary>
    /// 添加或修改
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task AddOrEditAsync(AddOrEditUserRequest dto)
    {
        var user = new SysUser();
        if (!string.IsNullOrWhiteSpace(dto.Id))
        {
            user = await _dbContext.SysUser.FirstOrDefaultAsync(sm => sm.Id == dto.Id);
            if (user == null)
                throw new UserFriendlyException("用户不存在");

            var exist = await _dbContext.SysUser.AnyAsync(x => x.LoginName == dto.LoginName && x.Id != dto.Id);
            if (exist)
                throw new UserFriendlyException("已存在的同名登录账号");
        }
        else
        {
            var exist = await _dbContext.SysUser.AnyAsync(x => x.LoginName == dto.LoginName);
            if (exist)
                throw new UserFriendlyException("已存在的同名登录账号");
        }

        user.SetBaseInfo(dto.DeptId, dto.LoginName, dto.UserName, dto.PhoneNumber, dto.Remark);
        user.Status = dto.Status;

        if (dto.Id.IsNullOrWhiteSpace())
        {
            user.RestPassword();
            await _dbContext.SysUser.AddAsync(user);
        }
        else
        {
            _dbContext.SysUser.Update(user);
        }

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 编辑角色
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task EditRoleAsync(EditUserRoleRequest dto)
    {
        var user = await _dbContext.SysUser.FirstOrDefaultAsync(sm => sm.Id == dto.UserId);
        if (user == null)
            throw new UserFriendlyException("用户不存在");

        var userRoleList = await _dbContext.SysUserRole.Where(sr => sr.UserId == dto.UserId).ToListAsync();
        if (userRoleList.Any())
        {
            _dbContext.SysUserRole.RemoveRange(userRoleList);
        }

        var userRole = dto.RoleIds.Select(r => new SysUserRole(dto.UserId, r));
        await _dbContext.SysUserRole.AddRangeAsync(userRole);

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="userId"></param>
    public async Task DeleteAsync(string userId)
    {
        var user = await _dbContext.SysUser.Where(x => x.Id == userId).FirstAsync();

        _dbContext.SysUser.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 设置状态
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task EditStatusAsync(EditUserStatusRequest dto)
    {
        var user = await _dbContext.SysUser.FirstOrDefaultAsync(sm => sm.Id == dto.UserId);
        if (user == null)
            throw new UserFriendlyException("用户不存在");

        user.Status = dto.Status;
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 设置密码
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task EditPassword(EditUserPasswordRequest dto)
    {
        var user = await _dbContext.SysUser.FirstOrDefaultAsync(sm => sm.Id == dto.UserId);
        if (user == null)
            throw new UserFriendlyException("用户不存在");
        user.SetPassword(dto.NewPassword);

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 修改我的密码
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task EditMyPassword(EditMyPasswordRequest dto)
    {
        var userId = _operatorProvider.GetOperator().OperatorId;
        var user = await _dbContext.SysUser.FirstOrDefaultAsync(sm => sm.Id == userId);
        if (user == null)
            throw new UserFriendlyException("用户不存在");

        if (!user.VerityPassword(dto.Password))
            throw new UserFriendlyException("旧密码错误");

        user.SetPassword(dto.NewPassword);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 设置用户基本信息
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task EditInfo(EditUserInfoRequest dto)
    {
        var userId = _operatorProvider.GetOperator().OperatorId;
        var user = await _dbContext.SysUser.FirstOrDefaultAsync(sm => sm.Id == userId);
        if (user == null)
            throw new UserFriendlyException("用户不存在");

        user.UserName = dto.UserName;
        user.PhoneNumber = dto.PhoneNumber;
        user.Avatar = dto.Avatar;

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<PageResult<UserPageResult>> PageAsync(UserPageRequest dto)
    {
        var total = new RefAsync<int>(0);
        var rows = await GetUserQueryable(dto).ToPageListAsync(dto.PageNo, dto.PageSize, total);

        var result = new PageResult<UserPageResult>
        {
            TotalCount = total,
            Rows = _mapper.Map<List<UserPageResult>>(rows),
        };
        return result;
    }

    /// <summary>
    /// 用户拥有的角色
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<string>> HasRoleAsync(string userId)
    {
        return await _baseQuery.Db.Queryable<SysUser, SysUserRole, SysRole>((u, ur, r) =>
                new JoinQueryInfos(JoinType.Inner, u.Id == ur.UserId,
                    JoinType.Inner, ur.RoleId == r.Id && r.Status == StarkConst.StatusYes))
            .Where(u => u.Id == userId)
            .Select((u, ur, r) => r.Id).ToListAsync() ?? new List<string>();
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<UserInfoResult> GetUserAsync(string userId)
    {
        return await _baseQuery.Db.Queryable<SysUser, SySDept>((u, d) =>
                new JoinQueryInfos(JoinType.Inner, u.DeptId == d.Id))
            .Where(u => u.Id == userId)
            .Select((u, d) => new UserInfoResult()
            {
                Id = u.Id,
                DeptId = u.DeptId,
                DeptName = d.DeptName,
                LoginName = u.LoginName,
                UserName = u.UserName,
                PhoneNumber = u.PhoneNumber,
                Avatar = u.Avatar,
                Status = u.Status,
                CreateTime = u.CreateTime,
                RoleNames = SqlFunc.Subqueryable<SysUserRole>().LeftJoin<SysRole>((m, n) => m.RoleId == n.Id).Where(m => m.UserId == u.Id).SelectStringJoin((m, n) => n.Name, ",")
            }).FirstAsync();
    }

    private ISugarQueryable<UserPageResult> GetUserQueryable(UserPageRequest dto)
    {
        return _baseQuery.Db.Queryable<SysUser, SySDept, SysUserRole, SysRole>((u, d, ur, r) =>
                new JoinQueryInfos(JoinType.Inner, u.DeptId == d.Id,
                    JoinType.Left, u.Id == ur.UserId,
                    JoinType.Left, ur.RoleId == r.Id))
            .WhereIF(!dto.DeptId.IsNullOrWhiteSpace(), d => d.Id == dto.DeptId)
            .WhereIF(!dto.RoleId.IsNullOrWhiteSpace(), r => r.Id == dto.RoleId)
            .WhereIF(!dto.UserName.IsNullOrWhiteSpace(), u => u.UserName.Contains(dto.UserName!))
            .WhereIF(!dto.Status.IsNullOrWhiteSpace(), u => u.Status == dto.Status)
            .GroupBy((u, d, ur, r) => new
            {
                u.Id
            })
            .OrderBy((u, d, ur, r) => u.CreateTime, OrderByType.Desc)
            .Select((u, d, ur, r) => new UserPageResult
            {
                Id = u.Id,
                DeptId = u.DeptId,
                DeptName = d.DeptName,
                LoginName = u.LoginName,
                UserName = u.UserName,
                Avatar = u.Avatar,
                PhoneNumber = u.PhoneNumber,
                Status = u.Status,
                Remark = u.Remark,
                CreateTime = u.CreateTime,
                RoleNames = SqlFunc.MappingColumn(r.Name, " IFNULL(GROUP_CONCAT(r.Name),'') "),
                //RoleNames = SqlFunc.Subqueryable<SysUserRole>().LeftJoin<SysRole>((m, n) => m.RoleId == n.Id).Where(m => m.UserId == u.Id).SelectStringJoin((m, n) => n.Name, ",")
            });
    }
}