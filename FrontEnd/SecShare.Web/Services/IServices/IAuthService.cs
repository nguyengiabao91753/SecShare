using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;

namespace SecShare.Web.Services.IServices;

public interface IAuthService
{
    Task<ResponseDTO?> Register(RegistrationRequestDto registrationRequestDto);
}
