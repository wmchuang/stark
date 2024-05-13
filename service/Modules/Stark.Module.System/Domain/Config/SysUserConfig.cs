using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stark.Starter.DDD.Infrastructure.EFCore.EntityBaseConfigurations;

namespace Stark.Module.System.Domain.Config;

public class SysUserConfig: AggregateRootConfiguration<SysUser>
{
    public override void Configure(EntityTypeBuilder<SysUser> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.DeptId).IsRequired().HasMaxLength(36).HasComment("部门Id");
        builder.Property(x => x.LoginName).IsRequired().HasMaxLength(50).HasComment("登录账号");
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(50).HasComment("用户名称");
        builder.Property(x => x.Avatar).IsRequired().HasMaxLength(500).HasComment("用户头像");
        builder.Property(x => x.Password).IsRequired().HasMaxLength(50).HasComment("密码");
        builder.Property(x => x.Salt).IsRequired().HasMaxLength(10).HasComment("加密盐");
        builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(11).HasComment("手机号码");
        builder.Property(x => x.Remark).IsRequired().HasMaxLength(200).HasComment("备注");
    }
}