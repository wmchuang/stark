using Microsoft.Extensions.Logging;

namespace Stark.Starter.Web.Logger;

public class LogInfo
{
    /// <summary>
    /// 记录器类别名称
    /// </summary>
    public string LogName { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// 事件 Id
    /// </summary>
    public EventId EventId { get; set; }

    /// <summary>
    /// 日志消息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 异常对象
    /// </summary>
    public Exception Exception { get; set; }

    /// <summary>
    /// 当前状态值
    /// </summary>
    /// <remarks>可以是任意类型</remarks>
    public object State { get; set; }

    /// <summary>
    /// 请求地址
    /// </summary>
    public string RequestUrl { get; set; }

    /// <summary>
    /// 日志记录时间
    /// </summary>
    public DateTime LogDateTime { get; set; }
    
    /// <summary>
    /// 登录账号
    /// </summary>
    public string? LoginName { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string? UserName { get; set; }
}