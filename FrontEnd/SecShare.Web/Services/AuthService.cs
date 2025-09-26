using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;
using SecShare.Helper.Utils;
using SecShare.Web.Services.IServices;

namespace SecShare.Web.Services;

public class AuthService : IAuthService
{
    private readonly IBaseService _baseService;

    public AuthService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDTO?> Register(RegistrationRequestDto registrationRequestDto)
    {
        return await _baseService.SendAsync(new RequestDTO()
        {
            ApiType = SD.ApiType.POST,
            Data = registrationRequestDto,
            Url = SD.AuthAPIBase + "/api/auth/register"
        }, withBearer: false);
    }
}
