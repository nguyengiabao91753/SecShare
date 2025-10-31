using Microsoft.AspNetCore.Mvc;
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

    Task<Stream> GetFileAsync(Guid DocumentId, string UserId);

    Task<ResponseDTO> ListUserFileAsync(string UserId);

    Task<ResponseDTO> ListReceiveFileAsync(string UserId);

    Task<ResponseDTO> GetListUsersShared( string UserId, Guid docId);

    Task<ResponseDTO> GetReceiverActivity(string UserId, Guid docId);
}
