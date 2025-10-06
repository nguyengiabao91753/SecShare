using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;
using SecShare.Helper.Utils;
using SecShare.Web.Services.IServices;
using SecShare.Web.Services.IServices.IUserServices;

namespace SecShare.Web.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseService _baseService;
        
        public UserService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO> changePassword(ChangePasswordDto changePasswordDto)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = changePasswordDto,
                Url = $"{SD.AuthAPIBase}/api/auth/changePassword"
            }, withBearer: true);
        }

        public async Task<ResponseDTO> getUserInfor()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Data = null,
                Url = $"{SD.AuthAPIBase}/api/auth/getUserInfor"
            }, withBearer: true);
        }

        public async Task<ResponseDTO> updateInformation(UserDto userDto)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = userDto,
                Url = $"{SD.AuthAPIBase}/api/auth/updateInformation"
            }, withBearer: true);
        }
    }
}