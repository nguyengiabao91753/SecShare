using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;
using SecShare.Helper.Utils;
using SecShare.Web.Services.IServices;
using SecShare.Web.Services.IServices.IUserServices;

namespace SecShare.Web.Services
{
    public class EmailConfirmedService : IEmailConfirmedService
    {
        private readonly IBaseService _baseService;
        public EmailConfirmedService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO> CheckEmailConfirmed()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Data = null,
                Url = $"{SD.AuthAPIBase}/api/auth/checkEmailConfirmed"
            }, withBearer: true);
        }

        public async Task<ResponseDTO> SendOTP(OtpDto otp)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = otp,
                Url = $"{SD.AuthAPIBase}/api/auth/sendOTP"
            }, withBearer: true);
        }

        public async Task<ResponseDTO> VerifyOTP(OtpDto otp)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = otp,
                Url = $"{SD.AuthAPIBase}/api/auth/verifyOTP"
            }, withBearer: true);
        }

    }
}
