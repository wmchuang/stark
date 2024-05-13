using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations;

namespace Stark.Module.AI.Domain.Config;

public class AiWikiConfig : AggregateRootConfiguration<AiWiki>
{
    public override void Configure(EntityTypeBuilder<AiWiki> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.WikiName).IsRequired().HasMaxLength(50).HasComment("知识库名称");
        builder.Property(x => x.Description).IsRequired().HasMaxLength(100).HasComment("描述");
        builder.Property(x => x.ChatModelId).IsRequired().HasMaxLength(50).HasComment("会话模型ID");
        builder.Property(x => x.EmbeddingModelId).IsRequired().HasMaxLength(50).HasComment("向量模型ID");
        builder.Property(x => x.DbType).IsRequired().HasMaxLength(5).HasComment("保存位置");
        builder.Property(x => x.ConnectionString).IsRequired().HasMaxLength(200).HasComment("数据库连接地址");
    }
}