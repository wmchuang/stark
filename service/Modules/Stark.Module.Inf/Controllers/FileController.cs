using Microsoft.AspNetCore.Mvc;
using Stark.Module.Inf.Dtos;
using Stark.Module.Inf.Services;

namespace Stark.Module.Inf.Controllers;

/// <summary>
/// 文件
/// </summary>
public class FileController : BaseController
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// 文件上传
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<UploadResult> UploadAsync([FromForm]UploadRequest dto)
    {
        return _fileService.UploadAsync(dto);
    }
}