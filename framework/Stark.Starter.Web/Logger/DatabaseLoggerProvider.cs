using System.Collections.Concurrent;
using System.Runtime.Versioning;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Stark.Starter.Web.Logger;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("Database")]
public sealed class DatabaseLoggerProvider : ILoggerProvider, ISupportExternalScope
{
    private readonly DatabaseLoggerConfiguration _currentConfig;
    private IDatabaseLoggerStory _databaseLoggerStory;

    /// <summary>
    /// 数据库日志写入器作用域范围
    /// </summary>
    internal IServiceScope _serviceScope;

    public IExternalScopeProvider _scopeProvider = new LoggerExternalScopeProvider();

    private readonly ConcurrentDictionary<string, DatabaseLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 内存通道事件
    /// </summary>
    private readonly Channel<LogInfo> _channel;

    public DatabaseLoggerProvider(DatabaseLoggerConfiguration config)
    {
        _currentConfig = config;

        // 配置通道，设置超出默认容量后进入等待
        var boundedChannelOptions = new BoundedChannelOptions(300)
        {
            FullMode = BoundedChannelFullMode.Wait
        };

        // 创建有限容量通道
        _channel = Channel.CreateBounded<LogInfo>(boundedChannelOptions);
    }

    /// <summary>
    /// 设置服务提供器
    /// </summary>
    /// <param name="serviceProvider"></param>
    internal void SetServiceProvider(IServiceProvider serviceProvider)
    {
        // 解析服务作用域工厂服务
        var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

        // 创建服务作用域
        var serviceScope = serviceScopeFactory.CreateScope();

        // 基于当前作用域创建数据库日志写入器
        _databaseLoggerStory = serviceScope.ServiceProvider.GetRequiredService<IDatabaseLoggerStory>();

        // 创建长时间运行的后台任务，并将日志消息队列中数据写入存储中
        Task.Factory.StartNew(state => ReadAsync(), this, TaskCreationOptions.LongRunning);
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, name => new DatabaseLogger(name, this));

    public DatabaseLoggerConfiguration GetCurrentConfig() => _currentConfig;

    public void Write(LogInfo eventSource)
    {
        // 空检查
        if (eventSource == default)
        {
            throw new ArgumentNullException(nameof(eventSource));
        }

        // 写入存储器
        _channel.Writer.TryWrite(eventSource);
    }

    private async Task ReadAsync()
    {
        await foreach (var log in _channel.Reader.ReadAllAsync())
        {
             _databaseLoggerStory.SaveAsync(log);
        }
    }

    public void Dispose()
    {
        _loggers.Clear();

        // 释放数据库写入器作用域范围
        _serviceScope?.Dispose();
    }

    public void SetScopeProvider(IExternalScopeProvider scopeProvider)
    {
        _scopeProvider = scopeProvider ?? throw new ArgumentNullException(nameof(scopeProvider));
    }
}