using SecShare.Core.BaseClass;
using SecShare.Core.Document;
using SecShare.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Base.Document;
public interface IDocumentAPIService
{
    Task<ResponseDTO> UploadMyFileAsync(UploadMyFileDto uploadMyFile);
    Task<ResponseDTO> ShareFileAsync(ShareFileDto share, string UserId);

    Task<ResponseDTO> ReadFile(string DocumentId, string UserId);
}
