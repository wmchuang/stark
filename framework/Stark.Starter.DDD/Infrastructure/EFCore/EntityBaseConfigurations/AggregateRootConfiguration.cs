using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Infrastructure.EFCore.Modeling;
using Stark.Starter.DDD.Domain.Entities;

namespace Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations
{
    public class AggregateRootConfiguration<TEntity> : EntityConfiguration<TEntity> where TEntity : AggregateRoot
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);
            
            builder.ConfigureAggregateRoot();
        }
    }
}