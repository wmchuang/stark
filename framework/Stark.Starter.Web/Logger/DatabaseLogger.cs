using Microsoft.Extensions.Logging;
using Stark.Starter.Web.Logger;

public sealed class DatabaseLogger : ILogger
{
    private readonly DatabaseLoggerProvider _databaseLoggerProvider;
    private readonly string _name;

    public DatabaseLogger(string name, DatabaseLoggerProvider databaseLoggerProvider)
    {
        _name = name;
        _databaseLoggerProvider = databaseLoggerProvider;
    }

    /// <summary>
    /// 开始逻辑操作范围
    /// </summary>
    /// <typeparam name="TState">标识符类型参数</typeparam>
    /// <param name="state">要写入的项/对象</param>
    /// <returns><see cref="IDisposable"/></returns>
    public IDisposable BeginScope<TState>(TState state)
    {
        return _databaseLoggerProvider._scopeProvider.Push(state); // 将状态推送到范围提供者中  
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        var config = _databaseLoggerProvider.GetCurrentConfig();
        return config.IsEnable && logLevel >= config.MinimumLevel;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        LogInfo? logInfo = null;
        // 解析日志上下文数据
        _databaseLoggerProvider._scopeProvider.ForEachScope<object>((scope, ctx) =>
        {
            if (scope != null && scope is LogInfo context)
            {
                logInfo = context;
            }
        }, null);

        if (logInfo == null) return;

        logInfo.LogLevel = logLevel;
        logInfo.EventId = eventId;
        logInfo.State = state;
        logInfo.Message = logInfo.Exception?.Message ?? formatter(state, exception);
        // logInfo.Message = formatter(state, exception);

        _databaseLoggerProvider.Write(logInfo);
    }
}