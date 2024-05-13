using Microsoft.Extensions.Logging;

public class DatabaseLoggerConfiguration
{
    public bool IsEnable { get; set; }

    /// <summary>
    /// 最低日志记录级别
    /// </summary>
    public LogLevel MinimumLevel { get; set; } = LogLevel.Warning;
}