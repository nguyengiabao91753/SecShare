using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Base.Auth
{
    public interface IUserAPIService
    {
        Task<ResponseDTO> getUserInfor(string userid);

        Task<ResponseDTO> changePasswordAsync(ChangePasswordDto changePasswordDto, string UserId);

        Task<ResponseDTO> updateInformation(UserDto user, string userid);

        Task<ResponseDTO> getAllUsersShared(IEnumerable<string> listUserId);
    }
}
