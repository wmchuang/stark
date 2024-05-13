using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Stark.Starter.DDD.Infrastructure.EFCore;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseDb(
        [NotNull] this DbContextOptionsBuilder optionsBuilder,
        string connectionString,string dbType)
    {
        Check.NotNull<DbContextOptionsBuilder>(optionsBuilder, nameof (optionsBuilder));

        switch (dbType)
        {
            case "MySql":
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                break;
            case "Sqlite":
                optionsBuilder.UseSqlite(connectionString);
                break;
            default:
                throw new NotSupportedException($"不支持的数据库类型：{dbType}");
        }
        

        return optionsBuilder;
    }
}