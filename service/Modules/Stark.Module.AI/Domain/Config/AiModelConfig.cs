using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations;

namespace Stark.Module.AI.Domain.Config;

public class AiModelConfig: AggregateRootConfiguration<AiModel>
{
    public override void Configure(EntityTypeBuilder<AiModel> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(50).HasComment("模型描述");
        builder.Property(x => x.ModelType).IsRequired().HasMaxLength(50).HasComment("模型类型");
        builder.Property(x => x.EndPoint).IsRequired().HasMaxLength(100).HasComment("模型地址");
        builder.Property(x => x.ModelName).IsRequired().HasMaxLength(50).HasComment("模型名称");
        builder.Property(x => x.ModelKey).IsRequired().HasMaxLength(500).HasComment("秘钥");
    }
}