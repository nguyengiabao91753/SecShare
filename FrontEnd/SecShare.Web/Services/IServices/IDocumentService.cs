using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;

namespace SecShare.Web.Services.IServices;

public interface IDocumentService
{
    Task<ResponseDTO?> UploadMyFile(UploadMyFileDto uploadMyFileDto);

    Task<ResponseDTO?> ShareFile(ShareFileDto shareFileDto);

    Task<ResponseDTO?> ListFiles();

    
}
