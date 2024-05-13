using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Stark.Starter.Web.Filters;

namespace Stark.Starter.Web.Logger;

public static class DatabaseLoggerExtensions
{
    public static ILoggingBuilder AddDatabaseLogger<TDatabaseLoggerStory>(
        this ILoggingBuilder builder) where TDatabaseLoggerStory : class, IDatabaseLoggerStory
    {
        builder.AddConfiguration();

        // 注册数据库日志写入器
        builder.Services.TryAddTransient<IDatabaseLoggerStory, TDatabaseLoggerStory>();

        // 获取配置
        var configuration = builder.Services.GetConfiguration();
        var config = configuration.GetSection("Logging:Database").Get<DatabaseLoggerConfiguration>();

        builder.Services.Add(ServiceDescriptor.Singleton<ILoggerProvider>((serviceProvider) =>
        {
            // 数据库日志记录器提供程序
            var databaseLoggerProvider = new DatabaseLoggerProvider(config ?? new DatabaseLoggerConfiguration());
            databaseLoggerProvider.SetServiceProvider(serviceProvider);
            return databaseLoggerProvider;
        }));

        // 添加过滤器
        // builder.Services.Configure<MvcOptions>(opts => opts.Filters.Add<LoggerFilter>());

        return builder;
    }

    public static ILoggingBuilder AddDatabaseLogger<TDatabaseLoggerStory>(
        this ILoggingBuilder builder,
        Action<DatabaseLoggerConfiguration> configure) where TDatabaseLoggerStory : class, IDatabaseLoggerStory
    {
        builder.AddDatabaseLogger<TDatabaseLoggerStory>();
        builder.Services.Configure(configure);

        return builder;
    }
}