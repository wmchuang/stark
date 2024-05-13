namespace Stark.Module.Inf.Options;

/// <summary>
/// Oss存储配置
/// </summary>
public class OssInfo
{
    /// <summary>
    /// 上传节点
    /// </summary>
    public string Endpoint { get; set; }

    /// <summary>
    /// 访问密钥
    /// </summary>
    public string AccessKeyId { get; set; }

    /// <summary>
    /// 访问密钥
    /// </summary>
    public string AccessKeySecret { get; set; }

    /// <summary>
    /// 存储空间
    /// </summary>
    public string BucketName { get; set; }


    /// <summary>
    /// 前端访问域名
    /// </summary>
    public string WebUrl { get; set; }
}