using Microsoft.AspNetCore.Http;

namespace Stark.Module.Inf.Dtos;

public class UploadRequest
{
    /// <summary>
    /// 文件
    /// </summary>
    public IFormFile File { get; set; }
}