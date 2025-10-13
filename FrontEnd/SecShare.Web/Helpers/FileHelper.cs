using Microsoft.AspNetCore.Components.Forms;
using System.IO.Pipelines;

namespace SecShare.Web.Helpers;

public class FileHelper
{
    public async static Task<IFormFile> ConvertIBrowserFileToIFormFile(IBrowserFile browserFile)
    {
        if (browserFile == null)
            return null;
        try
        {

        
        // Mở stream đọc dữ liệu file
        using var stream = browserFile.OpenReadStream(maxAllowedSize: 100 * 1024 * 1024); // 100MB
        // Sao chép vào MemoryStream để lưu tạm
        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        // Tạo FormFile (thuộc Microsoft.AspNetCore.Http)
        var formFile = new FormFile(memoryStream, 0, memoryStream.Length, "file", browserFile.Name)
        {
            Headers = new HeaderDictionary(),
            ContentType = browserFile.ContentType
        };
            return formFile;

        }
        catch (Exception e)
        {

            return null;
        }
    }
}
