using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations;

namespace Stark.Module.System.Domain.Config;

public class SysRoleMenuConfig : EntityConfiguration<SysRoleMenu>
{
    public override void Configure(EntityTypeBuilder<SysRoleMenu> builder)
    {
        base.Configure(builder);
        
        builder.Property(x => x.RoleId).IsRequired().HasMaxLength(36).HasComment("角色ID");
        builder.Property(x => x.MenuId).IsRequired().HasMaxLength(36).HasComment("菜单ID");
    }
}