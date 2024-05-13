using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations;

namespace Stark.Module.System.Domain.Config;

public class SysUserRoleConfig: EntityConfiguration<SysUserRole>
{
    public override void Configure(EntityTypeBuilder<SysUserRole> builder)
    {
        base.Configure(builder);
        
        builder.Property(x => x.RoleId).IsRequired().HasMaxLength(36).HasComment("角色ID");
        builder.Property(x => x.UserId).IsRequired().HasMaxLength(36).HasComment("用户ID");
    }
}