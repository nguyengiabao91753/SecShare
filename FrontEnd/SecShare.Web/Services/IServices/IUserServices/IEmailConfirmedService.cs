using SecShare.Core.BaseClass;
using SecShare.Core.Dtos;

namespace SecShare.Web.Services.IServices.IUserServices
{
    public interface IEmailConfirmedService
    {
        Task<ResponseDTO> CheckEmailConfirmed(string username);

        Task<ResponseDTO> SendOTP(OtpDto otp);

        Task<ResponseDTO> VerifyOTP(OtpDto otp);
    }
}
