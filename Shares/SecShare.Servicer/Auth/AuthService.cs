using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SecShare.Base.Auth;
using SecShare.Core.Auth;
using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;
using SecShare.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Servicer.Auth;
public class AuthService : IAuthService
{
    private readonly IdentityApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(IdentityApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }
    public async Task<ResponseDTO> Register(RegistrationRequestDto registrationRequestDto)
    {
        ApplicationUser user = new()
        {
            UserName = registrationRequestDto.UserName,
            Email = registrationRequestDto.Email,
            NormalizedEmail = registrationRequestDto.Email.ToUpper(),
            PhoneNumber = registrationRequestDto.PhoneNumber
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
            if (result.Succeeded)
            {
                return new ResponseDTO
                {
                    IsSuccess = true,
                    Message = "User Registration Successful",
                    Code = "0",
                    Result = user.Id
                };
            }
            else
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Message = string.Join(", ", result.Errors.FirstOrDefault().Description),
                    Code = "-1",
                    Result = null
                };
            }
        }
        catch (Exception ex)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Message = ex.Message,
                Code = "-1",
                Result = null
            };
        }
    }
}
