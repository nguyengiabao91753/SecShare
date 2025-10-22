using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using SecShare.Base.Auth;
using SecShare.Core.Auth;
using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;
using SecShare.Helper.EmailHelper;
using SecShare.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Servicer.Auth
{
    public class OtpAPIService : IotpService
    {
        private readonly IdentityApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailHelper _emailHelper;
        public OtpAPIService(
            IdentityApplicationDbContext db, 
            UserManager<ApplicationUser> userManager, 
            IMemoryCache memoryCache, 
            IEmailHelper emailHelper
            )
        {
            _db = db;
            _userManager = userManager;
            _memoryCache = memoryCache;
            _emailHelper = emailHelper;
        }

        public async Task<ResponseDTO> sendOtpAsync(OtpDto otpDto)
        {
            if (string.IsNullOrWhiteSpace(otpDto.userEmail))
            {
                return new ResponseDTO { 
                    IsSuccess = false,
                    Code = Convert.ToString(-1),
                    Message = "User not found!"
                };
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(otpDto.userEmail);
                if (user != null)
                {
                    //Sinh Otp ngau nhien
                    var otp = new Random().Next(100000, 999999).ToString();

                    //Luu vao bo nho tam trong 30 phut
                    _memoryCache.Set($"otp_{user.Email}", otp, TimeSpan.FromMinutes(30));

                    // Gửi OTP qua email
                    var emailSent = await _emailHelper.SendEmailAsync(user.Email, otp);

                    return new ResponseDTO
                    {
                        IsSuccess = true,
                        Code = Convert.ToString(0),
                        Message = $"OTP was sent to user email.",
                        Result = otp
                    };
                }
                else
                {
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        Code = Convert.ToString(-1),
                        Message = "User not found!"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Code = Convert.ToString(-1),
                    Message = ex.Message,
                    Result = null
                };
            }

        }

        public async Task<ResponseDTO> verifyOtpAsync(OtpDto otpDto)
        {
            if (string.IsNullOrWhiteSpace(otpDto.userEmail))
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Code = Convert.ToString(-1),
                    Message = "User not found!"
                };
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(otpDto.userEmail);
                if (user == null)
                {
                    return new ResponseDTO { 
                        IsSuccess = false,
                        Code = Convert.ToString(-1),
                        Message = "User not found!"
                    };
                }

                var Otp = otpDto.otp;

                if (_memoryCache.TryGetValue($"otp_{user.Email}", out string? cachedOtp))
                {
                    if (cachedOtp.Equals(Otp, StringComparison.Ordinal))
                    {
                        _memoryCache.Remove($"otp_{user.Email}");
                        user.EmailConfirmed = true;
                        await _userManager.UpdateAsync(user);

                        return new ResponseDTO
                        {
                            IsSuccess = true,
                            Code = Convert.ToString(0),
                            Message = "Verify OTP successfully!",
                            Result = user.EmailConfirmed
                        };
                    }
                    else
                    {
                        return new ResponseDTO
                        {
                            IsSuccess = false,
                            Code = Convert.ToString(-1),
                            Message = "OTP is incorrect"
                        };
                    }
                }
                else
                {
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        Code = Convert.ToString(-1),
                        Message ="OTP hasn't been sent or expired!"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Code = Convert.ToString(-1),
                    Message = ex.Message,
                    Result = null
                };
            }

        }
    }
}
