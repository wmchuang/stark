using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations;

namespace Stark.Module.System.Domain.Config;

public class SySDeptConfig : AggregateRootConfiguration<SySDept>
{
    public override void Configure(EntityTypeBuilder<SySDept> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.DeptName).IsRequired().HasMaxLength(50).HasComment("部门名称");
        builder.Property(x => x.ParentDeptId).IsRequired().HasMaxLength(36).HasComment("父级部门id");
        builder.Property(x => x.TreePath).IsRequired().HasMaxLength(150).HasComment("节点路径");
        builder.Property(x => x.Sort).IsRequired().HasComment("排序");
        
    }
}