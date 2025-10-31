using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;

namespace SecShare.Web.Services.IServices;

public interface IDocumentService
{
    Task<ResponseDTO?> UploadMyFile(UploadMyFileDto uploadMyFileDto);

    Task<ResponseDTO?> ShareFile(ShareFileDto shareFileDto);

    Task<ResponseDTO?> ListFiles();

    Task<ResponseDTO?> GetListUsersShared(string docId);

    Task<ResponseDTO?> GetListDocShared();

    Task<ResponseDTO?> GetReceiverActivity(string docId);

}
