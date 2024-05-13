using Microsoft.Extensions.Options;
using Stark.Module.Inf.Options;

namespace Stark.Module.Inf.Services;

public class FileService
{
     protected static readonly List<string> allowImgExt = new() {".png", ".jpg", ".jpeg", ".gif", ".bmp"};
     protected static readonly List<string> allowVideoExt = new() {".flv", ".avi", ".mp4", ".mp3", ".wav"};
     protected static readonly List<string> allowFilesExt = new() {".rar", ".zip", ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".txt",".csv"};
     protected static readonly List<string> allowOtherExt = new() {".apk"};
     
     private string _folderName;

     public FileService(IOptions<FileSetting> options)
     {
          _folderName = options.Value.FolderName;
     }

     /// <summary>
     /// 获取文件目录
     /// </summary>
     /// <param name="extension"></param>
     /// <returns></returns>
     protected string GetFileFolder(string extension)
     {
          var folder = DateTime.Now.ToString("yyyyMMdd");
          var path = $"{_folderName}/{folder}";

          if (allowImgExt.Any(x => x == extension))
               return $"{path}/images";

          if (allowVideoExt.Any(x => x == extension))
               return $"{path}/videos";

          if (allowFilesExt.Any(x => x == extension))
               return $"{path}/files";

          if (allowOtherExt.Any(x => x == extension))
               return $"{path}/other";

          return string.Empty;
     }
}