using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;
using SecShare.Helper.Utils;
using SecShare.Web.Services.IServices;

namespace SecShare.Web.Services;

public class DocumentService : IDocumentService
{
    private readonly IBaseService _baseService;
    public DocumentService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDTO?> GetListUsersShared(string docId)
    {
        return await _baseService.SendAsync(new RequestDTO()
        {
            ApiType = SD.ApiType.GET,
            Data = docId,
            Url = SD.DocumentAPIBase + $"/api/document/getListUsersShare/{docId}"

        }, withBearer: true);
    }

    public async Task<ResponseDTO?> GetListDocShared()
    {
        return await _baseService.SendAsync(new RequestDTO()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.DocumentAPIBase + "/api/document/getListReceivedDoc"

        }, withBearer: true);
    }

    public async Task<ResponseDTO?> ListFiles()
    {
        return await _baseService.SendAsync(new RequestDTO()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.DocumentAPIBase + "/api/document/getListDoc"

        }, withBearer: true);
    }

    public async Task<ResponseDTO?> ShareFile(ShareFileDto shareFileDto)
    {
       return await _baseService.SendAsync(new RequestDTO()
       {
           ApiType = SD.ApiType.POST,
           Data = shareFileDto,
           Url = SD.DocumentAPIBase + "/api/document/ShareMyFile"

       }, withBearer: true);
    }

    public async Task<ResponseDTO?> UploadMyFile(UploadMyFileDto uploadMyFileDto)
    {
        return await _baseService.SendAsync(new RequestDTO()
        {
            ApiType = SD.ApiType.POST,
            Data = uploadMyFileDto,
            Url = SD.DocumentAPIBase + "/api/document/uploadMyFile",
            IsMultipart = true
            
        }, withBearer: true);
    }

    public async Task<ResponseDTO?> GetReceiverActivity(string docId)
    {
       return await _baseService.SendAsync(new RequestDTO()
       {
           ApiType = SD.ApiType.GET,
           Data = docId,
           Url = SD.DocumentAPIBase + $"/api/document/getReceiverActivity/{docId}"
       }, withBearer: true);
    }
}
