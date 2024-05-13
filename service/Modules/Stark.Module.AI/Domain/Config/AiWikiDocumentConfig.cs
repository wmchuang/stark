using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations;

namespace Stark.Module.AI.Domain.Config;

public class AiWikiDocumentConfig : EntityConfiguration<AiWikiDocument>
{
    public override void Configure(EntityTypeBuilder<AiWikiDocument> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.WikiId).IsRequired().HasMaxLength(36).HasComment("知识库标识");
        builder.Property(x => x.FileName).IsRequired().HasMaxLength(200).HasComment("文件名称");
        builder.Property(x => x.Path).IsRequired().HasMaxLength(200).HasComment("地址");
        builder.Property(x => x.Text).IsRequired().HasMaxLength(2000).HasComment("文本");
        builder.Property(x => x.Type).IsRequired().HasComment("类型 文件、网页");
        builder.Property(x => x.Status).IsRequired().HasComment("状态");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("创建时间");
    }
}