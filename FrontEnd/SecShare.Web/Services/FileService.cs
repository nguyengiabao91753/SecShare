using Newtonsoft.Json;
using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;
using SecShare.Helper.Utils;
using SecShare.Web.Dtos;
using SecShare.Web.Services.IServices;

namespace SecShare.Web.Services;

public class FileService : IFileService
{
    
    private readonly IBaseService _baseService;

    public FileService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<string?> GetTempViewLinkAsync(string documentId)
    {

        var response = await _baseService.SendAsync(new RequestDTO()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.DocumentAPIBase + $"/api/file/view/{documentId}"
        }, withBearer: true);
        if (!response.IsSuccess)
            return null;

        var url = Convert.ToString(response.Result);
        return url;
    }
}
