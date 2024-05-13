using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations;

namespace Stark.Module.System.Domain.Config;

/// <summary>
/// 
/// </summary>
public class SysLogVisitConfig: AggregateRootConfiguration<SysLogVisit>
{
    public override void Configure(EntityTypeBuilder<SysLogVisit> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.ActionName).IsRequired().HasMaxLength(50).HasComment("方法名称");
        builder.Property(x => x.RemoteIp).IsRequired().HasMaxLength(50).HasComment("IP地址");
        builder.Property(x => x.Location).IsRequired().HasMaxLength(50).HasComment("登录地点");
        builder.Property(x => x.Browser).IsRequired().HasMaxLength(50).HasComment("浏览器");
        builder.Property(x => x.Os).IsRequired().HasMaxLength(50).HasComment("操作系统");
        builder.Property(x => x.Elapsed).IsRequired().HasComment("操作用时");
        builder.Property(x => x.LoginName).IsRequired().HasMaxLength(50).HasComment("登录账号");
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(50).HasComment("用户名称");
      
    }
}