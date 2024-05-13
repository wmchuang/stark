
using Stark.Starter.DDD.Domain.Entities;

namespace Stark.Module.System.Domain;

/// <summary>
/// 错误日志
/// </summary>
public class SysLogEx : AggregateRoot
{
    /// <summary>
    /// 日志消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 堆栈信息
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>
    /// 请求地址
    /// </summary>
    public string? RequestUrl { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// 登录账号
    /// </summary>
    public string? LoginName { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string? UserName { get; set; }
}