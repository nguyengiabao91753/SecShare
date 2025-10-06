using Microsoft.AspNetCore.Http;
using SecShare.Core.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Base.File;
public interface IClouDinaryService
{
    Task<ResponseDTO> UploadFileAsync(IFormFile file, string PublicId = null, string fileFolder = "default_folder");

    Task<ResponseDTO> DeleteFileAsync(string PublicId);
}
