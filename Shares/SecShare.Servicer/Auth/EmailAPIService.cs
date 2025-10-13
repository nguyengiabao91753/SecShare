using Microsoft.AspNetCore.Identity;
using SecShare.Base.Auth;
using SecShare.Core.Auth;
using SecShare.Core.BaseClass;
using SecShare.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Servicer.Auth
{
    public class EmailAPIService : IEmailAPIService
    {
        private readonly IdentityApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public EmailAPIService(IdentityApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<ResponseDTO> checkEmailConfirmed(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return new ResponseDTO { 
                    IsSuccess = false,
                    Code = Convert.ToString(-1),
                    Message = "User not found!"
                };

            }

            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null) {
                    return new ResponseDTO {
                        IsSuccess = false,
                        Code = Convert.ToString(-1),
                        Message = "User not found!"
                    };
                }

                if (user.EmailConfirmed == false) {
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        Code = Convert.ToString(-1),
                        Message = "Email has not been verified yet.",
                        Result = user.Email
                    };
                }

                return new ResponseDTO { 
                    IsSuccess = true,
                    Code = Convert.ToString(0),
                    Message = "",
                    Result = user.Email
                };
            }
            catch (Exception ex) {
                return new ResponseDTO {
                    IsSuccess = false,
                    Code = Convert.ToString(-1),
                    Message = $"Error occured. Error: {ex.Message}"
                };
            }
        }
    }
}
