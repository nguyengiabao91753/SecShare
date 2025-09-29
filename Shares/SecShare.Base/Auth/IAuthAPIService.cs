
using SecShare.Core.Auth;
using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Base.Auth;
public interface IAuthAPIService
{
    Task<ResponseDTO> Register (RegistrationRequestDto registrationRequestDto);

    Task<ResponseDTO> Login(LoginRequestDto loginRequestDto);
}
