using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations;

namespace Stark.Module.System.Domain.Config;

public class SysMenuConfig : AggregateRootConfiguration<SysMenu>
{
    public override void Configure(EntityTypeBuilder<SysMenu> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(50).HasComment("角色名称");
        builder.Property(x => x.ParentId).IsRequired().HasMaxLength(36).HasComment("父菜单ID");
        builder.Property(x => x.Sort).IsRequired().HasComment("角色排序");
        builder.Property(x => x.Link).IsRequired().HasMaxLength(200).HasComment("是否为外链(有值则为外链)");
        builder.Property(x => x.Type).IsRequired().HasComment("菜单类型 0：目录，1：菜单，2：按钮");
        builder.Property(x => x.Code).IsRequired().HasMaxLength(50).HasComment("权限字符串");
        builder.Property(x => x.Icon).IsRequired().HasMaxLength(50).HasComment("菜单图标");
        builder.Property(x => x.Hidden).IsRequired().HasDefaultValue(false).HasComment("是否隐藏");
    }
}