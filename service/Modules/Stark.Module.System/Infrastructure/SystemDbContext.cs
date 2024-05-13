using Microsoft.EntityFrameworkCore;
using Stark.Module.System.Domain;
using Stark.Starter.DDD.Infrastructure.EFCore;

namespace Stark.Module.System.Infrastructure;

public class SystemDbContext : StarkDbContext<SystemDbContext>
{
    public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// 部门
    /// </summary>
    public DbSet<SySDept> SySDept { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    public DbSet<SysUser> SysUser { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    public DbSet<SysRole> SysRole { get; set; }

    /// <summary>
    /// 用户角色关联表
    /// </summary>
    public DbSet<SysUserRole> SysUserRole { get; set; }

    /// <summary>
    /// 菜单
    /// </summary>
    public DbSet<SysMenu> SysMenu { get; set; }

    /// <summary>
    /// 菜单角色关联表
    /// </summary>
    public DbSet<SysRoleMenu> SysRoleMenu { get; set; }

    /// <summary>
    /// 访问日志
    /// </summary>
    public DbSet<SysLogVisit> SysLogVisit { get; set; }

    /// <summary>
    /// 错误日志
    /// </summary>
    public DbSet<SysLogEx> SysLogEx { get; set; }

    /// <summary>
    /// 模型构建
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //使用当前类型所在的程序集，将 从当前类型所在的程序集加载所有配置应用到模型构建器中所有的配置信息并应用到模型构建
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}