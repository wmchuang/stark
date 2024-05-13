using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations;

namespace Stark.Module.AI.Domain.Config;

public class AiBotConfig : AggregateRootConfiguration<AiBot>
{
    public override void Configure(EntityTypeBuilder<AiBot> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50).HasComment("名称");
        builder.Property(x => x.Description).IsRequired().HasMaxLength(200).HasComment("描述");
        builder.Property(x => x.ChatModelId).IsRequired().HasMaxLength(50).HasComment("AI模型");
        builder.Property(x => x.Avatar).IsRequired().HasMaxLength(200).HasComment("知识库头像");
        builder.Property(x => x.Prompting).IsRequired().HasMaxLength(1000).HasComment("提示词");
        builder.Property(x => x.Temperature).IsRequired().HasPrecision(2, 1).HasComment("温度");
        builder.Property(x => x.Opening).IsRequired().HasMaxLength(200).HasComment("开场白");
        builder.Property(x => x.StartPrologues).IsRequired().HasComment("推荐问题");
        builder.Property(x => x.WikiIds).IsRequired().HasComment("标识集合");
    }
}