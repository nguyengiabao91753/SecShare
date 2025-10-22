using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using SecShare.Core.Dtos;
using SecShare.Web.Services.IServices;
using SecShare.Web.Services.IServices.IUserServices;

namespace SecShare.Web.Components.Pages.client.Auth.Login;

public partial class Login
{
    [Inject]
    private IAuthService _authService { get; set; }
    [Inject]
    private IEmailConfirmedService _emailConfirmedService { get; set; }
    [Inject]
    private ITokenProvider _tokenProvider { get; set; }

    [SupplyParameterFromForm]
    private LoginRequestDto? model { get; set; }
    private string errorMessage { get; set; } = "";
    private string? infoMessage;
    private EditContext editContext;
    private bool isSubmitting = false;
    private bool showPassword = false;
    protected override void OnInitialized()
    {
        model = new();
        editContext = new EditContext(model);
    }


    private async Task HandleLogin()
    {
        var checkResponse = await _emailConfirmedService.CheckEmailConfirmed(model.Username);
        if (checkResponse != null && checkResponse.IsSuccess)
        {
            isSubmitting = true;
            try
            {
                var rs = await _authService.Login(model);
                if (!rs.IsSuccess)
                {
                errorMessage = $"Login failed: {rs.Message}";
                }
                else
                {
                LoginResponseDto loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(rs.Result));
                _tokenProvider.SetTokenAsync(loginResponse.Token);

                Navigation.NavigateTo("/myspace"); // Email đã xác nhận
                }
            }
            finally
            {
            isSubmitting = false;
            }
        }
        else
        {
            var email = checkResponse.Result?.ToString();
            if (!String.IsNullOrEmpty(email))
            {
                infoMessage = "Please verify your email before continuing...";
                StateHasChanged(); // Cập nhật UI


                Navigation.NavigateTo($"/verify-otp?email={Uri.EscapeDataString(email)}", forceLoad: true);
            }
            else
            {
                errorMessage = $"Login failed. Account not found.";
            }
        }
    }


       

    private void TogglePassword()
    {
        showPassword = !showPassword;
    }

}
