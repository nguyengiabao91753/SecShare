using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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

namespace SecShare.Servicer.Auth
{
    public class UserService : IUserService
    {
        private readonly IdentityApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IdentityApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
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

                var userDTO = new UserDTO
                {
                    userId = userid,
                    name = user.UserName,
                    email = user.Email,
                    phone = user.PhoneNumber
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
    }
}
