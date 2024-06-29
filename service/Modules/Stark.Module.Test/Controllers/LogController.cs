using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Stark.Module.Test.Controllers;

/// <summary>
/// 日志
/// </summary>
public class LogController : BaseController
{
    private readonly ILogger<LogController> _logger;

    public LogController(ILogger<LogController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 测试日志
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public string TestLogSeq()
    {
        _logger.LogTrace("this is a test log for trace level.");
        _logger.LogDebug("this is a test log for debug level.");
        _logger.LogInformation("this is a test log for information level.");
        _logger.LogWarning("this is a test log for warning level.");
        _logger.LogError("this is a test log for error level.");
        _logger.LogCritical("this is a test log for critical level.");
        
        return "ok";
    }
}