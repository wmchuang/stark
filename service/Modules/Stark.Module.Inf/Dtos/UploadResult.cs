namespace Stark.Module.Inf.Dtos;

public class UploadResult
{
    /// <summary>
    /// 文件url路径
    /// </summary>
    public string FileUrl => WebUrl + "/" + Path;

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; }
    
    /// <summary>
    /// 文件存放目录
    /// </summary>
    public string FullName { get; set; }
    
    /// <summary>
    /// 文件地址
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 域名
    /// </summary>
    public string WebUrl { get; set; }
}