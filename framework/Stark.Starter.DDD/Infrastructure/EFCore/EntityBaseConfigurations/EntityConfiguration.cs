using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Domain.Entities;

namespace Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations;

public class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        var genericType = typeof(TEntity);
        builder.HasKey(x => x.Id);
        builder.ToTable(genericType.Name);
        builder.Property(x => x.Id).IsRequired().HasMaxLength(36).HasComment("主键");
    }
}