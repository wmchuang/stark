using Aliyun.OSS;
using Aliyun.OSS.Util;
using Microsoft.Extensions.Options;
using Stark.Module.Inf.Dtos;
using Stark.Module.Inf.Options;
using Volo.Abp;

namespace Stark.Module.Inf.Services;

public class OssService : FileService, IFileService
{
    private readonly OssInfo _ossOptions;
    private string _folderName;

    public OssService(IOptions<FileSetting> options) : base(options)
    {
        _ossOptions = options.Value.OssInfo ?? throw new UserFriendlyException("Oss配置错误");
    }

    /// <summary>
    /// 上传
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task<UploadResult> UploadAsync(UploadRequest dto)
    {
        var client = new OssClient(_ossOptions.Endpoint, _ossOptions.AccessKeyId, _ossOptions.AccessKeySecret);
        try
        {
            await using var fileStream = dto.File.OpenReadStream();
            var md5 = OssUtils.ComputeContentMd5(fileStream, fileStream.Length);
            var path = GetFilePath(dto);
            if (string.IsNullOrWhiteSpace(path))
                throw new UserFriendlyException("不允许上传此类型文件");

            //将文件md5值赋值给meat头信息，服务器验证文件MD5
            var objectMeta = new ObjectMetadata
            {
                ContentMd5 = md5,
            };

            await Task.Run(() => { client.PutObject(_ossOptions.BucketName, path, fileStream, objectMeta); });
            return new UploadResult { WebUrl = _ossOptions.WebUrl, Path = path };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 获取文件路径
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    private string GetFilePath(UploadRequest dto)
    {
        var ext = Path.GetExtension(dto.File.FileName).ToLower();
        var fileName = $"{Guid.NewGuid()}{ext}";
        var path = GetFileFolder(ext);
        if (string.IsNullOrWhiteSpace(path))
            return string.Empty;

        return $"{path}/{fileName}";
    }
}