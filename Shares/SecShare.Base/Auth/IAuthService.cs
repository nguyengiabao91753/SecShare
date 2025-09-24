using SecShare.Core.Dtos;
using SecShare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Application.Auth.AuthService;
public interface IAuthService
{
    Task<Response<string>> Register (RegistrationRequestDto registrationRequestDto);
}
