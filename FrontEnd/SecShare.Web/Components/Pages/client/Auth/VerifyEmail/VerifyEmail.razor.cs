using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using SecShare.Base.Auth;
using SecShare.Core.Dtos;
using SecShare.Web.Services;
using SecShare.Web.Services.IServices.IUserServices;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SecShare.Web.Components.Pages.client.Auth.VerifyEmail
{
    public partial class VerifyEmail
    {
        [Inject]
        private IUserService _userService { get; set; }
        [Inject] 
        private IEmailConfirmedService emailConfirmed { get; set; }
        private OtpDto? OtpDto { get; set; }
        private string userEmail;
        private UserDto? userModel {  get; set; }
        string errorMessage = "";
        bool isResending = false;
        string infoMessage = "";
        private string otpCode = string.Empty;
        private string MaskedEmail => string.IsNullOrEmpty(userEmail) ? "" : MaskEmail(userEmail);
        private int resendCountdown = 0;
        private Timer? resendTimer;

        private string MaskEmail(string email)
        {
            var atIndex = email.IndexOf('@');
            if (atIndex < 0) return email;
            var namePart = email[..atIndex];
            var domainPart = email[atIndex..];
            if (namePart.Length <= 3)
                return new string('*', namePart.Length) + domainPart;
            return namePart[..1] + new string('*', namePart.Length - 4) + namePart[^3..] + domainPart;
        }


        protected override async Task OnInitializedAsync()
        {
            var uri = new Uri(Navigation.Uri);
            var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            if (queryParams.TryGetValue("email", out var emailValue))
            {
                userEmail = emailValue;
            }

            if (!string.IsNullOrEmpty(userEmail))
            {
                await SendOTP(userEmail);
            }
        }

        private async Task SendOTP(string userEmail)
        {
            if (isResending|| string.IsNullOrEmpty(userEmail))
            {
                return;
            }

            isResending = true;
            errorMessage = string.Empty;
            infoMessage = "Sending.....";
            StateHasChanged();

            try
            {
                OtpDto = new OtpDto { 
                    userEmail = userEmail,
                    otp = null
                };
                var response = await emailConfirmed.SendOTP(OtpDto);
                if (response != null &&response.IsSuccess) {
                    infoMessage = "OTP code was sent to your email.";
                }
                else
                {
                    infoMessage = "";
                    errorMessage = "Error occured. Send OTP failed.";
                }
            }
            catch (Exception ex) {
                errorMessage = $"Error while sending OTP. Error: {ex.Message}";
            }
            finally
            {
                isResending = false;
                await Task.Delay(4000);
                infoMessage = errorMessage = string.Empty;
                StateHasChanged();
            }

        }

        private async Task VerifyOTP()
        {
            if (string.IsNullOrEmpty(otpCode))
            {
                errorMessage = "Please enter your OTP.";
                return;
            }

            try
            {
                OtpDto = new OtpDto
                {
                    otp = otpCode,
                    userEmail = userEmail,
                };
                var result = await emailConfirmed.VerifyOTP(OtpDto);
                if (!result.IsSuccess)
                {
                    errorMessage = result.Message; // lấy message từ API
                    StateHasChanged(); // refresh UI
                    await Task.Delay(3000); // đợi 3 giây
                    errorMessage = null;
                    StateHasChanged();
                    return;
                }

                // Thành công thì hiện message từ API
                infoMessage = result.Message;
                StateHasChanged();


                // Chờ 3 giây rồi redirect
                await Task.Delay(3000);
                Navigation.NavigateTo("/login");
            }
            catch (Exception ex) { 
                errorMessage= $"Error while verifying OTP. Error: {ex.Message}";
            }
        }

        private async Task ResendOTP()
        {
            await SendOTP(userEmail);
        }

        private void StartResendCountdown(int seconds)
        {
            resendCountdown = seconds;
            resendTimer?.Dispose();

            resendTimer = new Timer(1000);
            resendTimer.Elapsed += async (_, _) =>
            {
                if (resendCountdown > 0)
                {
                    resendCountdown--;
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    resendTimer?.Stop();
                    resendTimer?.Dispose();
                }
            };
            resendTimer.Start();
        }

        public void Dispose()
        {
            resendTimer?.Dispose();
        }
    }
}
