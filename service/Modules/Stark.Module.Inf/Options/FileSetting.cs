namespace Stark.Module.Inf.Options;

/// <summary>
/// 文件配置
/// </summary>
public class FileSetting
{
    /// <summary>
    /// 文件夹名称
    /// </summary>
    public string FolderName { get; set; }
    
    /// <summary>
    /// 是否是用oss
    /// </summary>
    public bool IsUseOss { get; set; }
    
    /// <summary>
    /// 本地存储配置
    /// </summary>
    public LocalInfo? LocalInfo { get; set; }
    
    /// <summary>
    /// oss存储配置
    /// </summary>
    public OssInfo? OssInfo { get; set; }
}