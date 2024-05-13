using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations;

namespace Stark.Module.System.Domain.Config;

public class SysRoleConfig : AggregateRootConfiguration<SysRole>
{
    public override void Configure(EntityTypeBuilder<SysRole> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(50).HasComment("角色名称");
        builder.Property(x => x.Key).IsRequired().HasMaxLength(50).HasComment("角色权限");
        builder.Property(x => x.Sort).IsRequired().HasComment("角色排序");
        builder.Property(x => x.Remark).IsRequired().HasMaxLength(200).HasComment("备注");
        builder.Property(x => x.DataScope).IsRequired().HasComment("数据范围");
    }
}