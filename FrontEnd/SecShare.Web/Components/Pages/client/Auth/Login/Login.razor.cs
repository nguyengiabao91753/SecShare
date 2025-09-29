using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using SecShare.Core.Dtos;
using SecShare.Web.Services.IServices;

namespace SecShare.Web.Components.Pages.client.Auth.Login;

public partial class Login
{
    [Inject]
    private IAuthService _authService { get; set; }

    [Inject]
    private ITokenProvider _tokenProvider { get; set; }

    [SupplyParameterFromForm]
    private LoginRequestDto? model { get; set; }
    private string errorMessage { get; set; } = "";
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

                Navigation.NavigateTo("/");
            }


        }
        finally
        {
            isSubmitting = false;
        }
    }

    private void TogglePassword()
    {
        showPassword = !showPassword;
    }

}
