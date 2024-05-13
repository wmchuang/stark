using Stark.Module.Inf.Dtos;

namespace Stark.Module.Inf.Services;

public interface IFileService
{
    Task<UploadResult> UploadAsync(UploadRequest dto);
}