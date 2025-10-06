using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SecShare.Base.Auth;
using SecShare.Core.Auth;
using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;
using SecShare.Helper.Security;
using SecShare.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Servicer.Auth
{
    public class UserAPIService : IUserAPIService
    {
        private readonly IdentityApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserAPIService(IdentityApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<ResponseDTO> changePasswordAsync(ChangePasswordDto changePasswordDto, string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Code = Convert.ToString(-1),
                    Message = "User not found"
                };
            }

            try
            {
                var user = await _userManager.FindByIdAsync(UserId);
                if (user == null)
                {
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        Code = Convert.ToString(-1),
                        Message = "User not found!"
                    };
                }

                var isValid = await _userManager.CheckPasswordAsync(user, changePasswordDto.OldPassword);

                if (!isValid)
                {
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        Code = Convert.ToString(-1),
                        Message = "Old password is incorrect!",
                        Result = null
                    };
                }

                var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
                if (result.Succeeded && user.Id != null)
                {
                    // Tạo cặp khóa RSA
                    (user.PublicKey, user.RsaPrivateKeyEncrypted) = RsaKeyPairHelper.GenerateKeyPair(user.PasswordHash);
                    await _userManager.UpdateAsync(user);
                    return new ResponseDTO
                    {
                        IsSuccess = true,
                        Message = "Change Password Successfully!",
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
            catch (Exception ex) {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Code = Convert.ToString(-1),
                    Message = ex.Message,
                    Result = null
                };
            }
        }

        public async Task<ResponseDTO> getUserInfor(string userid)
        {
            
            if (string.IsNullOrEmpty(userid))
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Code = Convert.ToString(-1),
                    Message = "User not found!",
                };
            }

            try
            {
                var user = await _userManager.FindByIdAsync(userid);
                if (user == null) {
                    return new ResponseDTO
                    {
                        IsSuccess =false,
                        Code = Convert.ToString(-1),
                        Message = "User not found!"
                    };
                }

                var userDTO = new UserDto
                {
                   
                    ID = user.Id,
                    Name = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                return new ResponseDTO
                {
                    IsSuccess = true,
                    Code = Convert.ToString(0),
                    Result = userDTO
                };
            }
            catch (Exception ex) {
                return new ResponseDTO 
                { 
                    IsSuccess=false,
                    Code = Convert.ToString(-1),
                    Message= ex.Message,
                    Result = null
                };
            }
        }

        public async Task<ResponseDTO> updateInformation(UserDto user, string userid)
        {
            if (string.IsNullOrEmpty(userid))
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Code = Convert.ToString(-1),
                    Message = "User not found!",
                };
            }

            try
            {
                var User = await _userManager.FindByIdAsync(userid);
                if (User == null)
                {
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        Code = Convert.ToString(-1),
                        Message = "User not found!"
                    };
                }

                // Gán lại thông tin từ DTO vào entity
                User.UserName = user.Name;
                User.PhoneNumber = user.PhoneNumber;
                var result = await _userManager.UpdateAsync(User);
                if (result.Succeeded && User.Id != null)
                {
                    
                    await _userManager.UpdateAsync(User);
                    return new ResponseDTO
                    {
                        IsSuccess = true,
                        Message = "Your Informaiton Updated Successfully!",
                        Code = "0",
                        Result = User.Id
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
                    Code = Convert.ToString(-1),
                    Message = ex.Message,
                    Result = null
                };
            }
        }
    }
}
