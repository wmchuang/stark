using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Stark.Module.Inf.Dtos;
using Stark.Module.Inf.Options;
using Volo.Abp;

namespace Stark.Module.Inf.Services;

public class LocalService : FileService, IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _accessor;
    private readonly LocalInfo? _localInfo;

    public LocalService(IOptions<FileSetting> options, IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor accessor) : base(options)
    {
        try
        {
            _webHostEnvironment = webHostEnvironment;
            _accessor = accessor;
            _localInfo = options.Value.LocalInfo;
        }catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new UserFriendlyException(ex.Message);
        }
    }

    public async Task<UploadResult> UploadAsync(UploadRequest dto)
    {
        try
        {
            await using var fileStream = dto.File.OpenReadStream();

            var ext = Path.GetExtension(dto.File.FileName).ToLower();
            var path = GetFileFolder(ext);
            if (string.IsNullOrWhiteSpace(path))
                throw new UserFriendlyException("不允许上传此类型文件");

            var fileName = $"{Guid.NewGuid()}{ext}";

            var rootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrWhiteSpace(rootPath))
                throw new UserFriendlyException("rootPath不可为空");
            
            var folderPath = Path.Combine(rootPath, path);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            
            
            var fileInfo = new FileInfo(Path.Combine(folderPath, fileName));
            await using (var fs = fileInfo.Create())
            {
                await dto.File.CopyToAsync(fs);
                fs.Flush();
            }

            var webUrl = _localInfo?.WebUrl ??
                         _accessor.HttpContext!.Request.Scheme + "://" + _accessor.HttpContext.Request.Host.Value;
            
            return new UploadResult
            {
                WebUrl = webUrl, 
                Path = Path.Combine(path, fileName),
                FileName = dto.File.FileName,
                FullName = fileInfo.FullName
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}