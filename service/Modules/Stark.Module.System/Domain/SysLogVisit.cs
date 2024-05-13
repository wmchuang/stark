using Stark.Starter.DDD.Domain.Entities;

namespace Stark.Module.System.Domain;

/// <summary>
/// 访问日志
/// </summary>
public class SysLogVisit : AggregateRoot
{
    /// <summary>
    /// 方法名称
    ///</summary>
    public string? ActionName { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    public string? RemoteIp { get; set; }

    /// <summary>
    /// 登录地点
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    public string? Browser { get; set; }
    
    /// <summary>
    /// 系统
    /// </summary>
    public string? Os { get; set; }

    /// <summary>
    /// 操作用时
    /// </summary>
    public long? Elapsed { get; set; }

    /// <summary>
    /// 登录账号
    /// </summary>
    public string? LoginName { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string? UserName { get; set; }
}