using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations;

namespace Stark.Module.System.Domain.Config;

public class SysLogExConfig: AggregateRootConfiguration<SysLogEx>
{
    public override void Configure(EntityTypeBuilder<SysLogEx> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Message).IsRequired().HasMaxLength(2000).HasComment("日志消息");
        builder.Property(x => x.StackTrace).IsRequired().HasComment("堆栈信息");
        builder.Property(x => x.RequestUrl).IsRequired().HasMaxLength(500).HasComment("请求地址");
        builder.Property(x => x.Source).IsRequired().HasComment("来源");
        builder.Property(x => x.LoginName).IsRequired().HasMaxLength(50).HasComment("登录账号");
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(50).HasComment("用户名称");
    }
}