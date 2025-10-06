using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;
using SecShare.Helper.Utils;
using SecShare.Web.Services.IServices;

namespace SecShare.Web.Services.IServices.IUserServices
{
    public interface IUserService
    {
        Task<ResponseDTO> getUserInfor();

        Task<ResponseDTO> changePassword(ChangePasswordDto changePasswordDto);
        Task<ResponseDTO> updateInformation(UserDto userDto);
    }
}
