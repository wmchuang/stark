
using Stark.Starter.DDD.Domain.Entities;



namespace Stark.Module.Cms.Domain;

/// <summary>
/// 站点表
/// </summary>
public class CmsSite : Entity
{
    /// <summary>
    /// 站点名称
    /// </summary>
    public string SiteName { get; set; } = string.Empty;
    
    /// <summary>
    /// 站点缩略图地址
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// 站点关键词
    /// </summary>
    public string Keywords { get; set; } = string.Empty;
    
    /// <summary>
    /// 站点简介
    /// </summary>
    public string Description { get; set; } = string.Empty;
}