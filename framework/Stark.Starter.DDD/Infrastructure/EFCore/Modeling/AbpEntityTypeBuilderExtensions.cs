using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.Core.Const;
using Stark.Starter.DDD.Domain;
using Stark.Starter.DDD.Domain.Entities;
using Volo.Abp;

namespace Stark.Starter.DDD.Infrastructure.EFCore.Modeling;

public static class AbpEntityTypeBuilderExtensions
{
    public static void ConfigureByConvention(this EntityTypeBuilder b)
    {
        //配置软删除
        b.TryConfigureSoftDelete();
        //配置可用状态
        b.TryConfigureStatus();
    }

    public static void ConfigureSoftDelete<T>(this EntityTypeBuilder<T> b)
        where T : class, ISoftDelete
    {
        b.As<EntityTypeBuilder>().TryConfigureSoftDelete();
    }

    public static void TryConfigureSoftDelete(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<ISoftDelete>())
        {
            b.Property(nameof(ISoftDelete.IsDeleted))
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName(nameof(ISoftDelete.IsDeleted))
                .HasComment("是否删除");
        }
    }
    
    public static void TryConfigureStatus(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<ISoftDelete>())
        {
            b.Property(nameof(IEntityStatus.Status))
                .IsRequired()
                .HasMaxLength(1)
                .HasDefaultValue(StarkConst.StatusYes)
                .HasComment("状态 Y:启用 N:禁用");
        }
    }

    public static void ConfigureAggregateRoot<T>(this EntityTypeBuilder<T> b)
        where T : AggregateRoot
    {
        b.Property(x => x.CreateBy).IsRequired().HasMaxLength(50).HasComment("创建人");
        b.Property(x => x.CreateName).IsRequired().HasMaxLength(200).HasComment("创建人名称");
        b.Property(x => x.CreateTime).IsRequired().HasComment("创建时间");
        b.Property(x => x.UpdateBy).IsRequired().HasMaxLength(50).HasComment("最后修改人");
        b.Property(x => x.UpdateName).IsRequired().HasMaxLength(200).HasComment("修改人名称");
        b.Property(x => x.UpdateTime).IsRequired().HasComment("最后修改时间");
    }

}