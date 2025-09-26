using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SecShare.Core.Dtos;
using SecShare.Web.Services.IServices;

namespace SecShare.Web.Components.Pages.client.Auth.Register;

public partial class Register
{

    [Inject]
    private IAuthService _authService { get; set; } = default!;
    [SupplyParameterFromForm]
    private RegistrationRequestDto? registerModel { get; set; }
    private string errorMessage = "";
    private bool isSubmitting = false;


    private bool showPassword = false;
    private bool showConfirmPassword = false;
    private EditContext editContext;
    protected override void OnInitialized()
    {
        registerModel = new RegistrationRequestDto();
        editContext = new EditContext(registerModel);
    }
    //public Register()
    //{
        
    //}
    private async Task HandleRegister()
    {
        isSubmitting = true;
        try
        {
            var result = await _authService.Register(registerModel);
            if (!result.IsSuccess)
            {
                errorMessage = $"Registration failed: {result.Message}";
                return;
            }

            Navigation.NavigateTo("/");
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

    private void ToggleConfirmPassword()
    {
        showConfirmPassword = !showConfirmPassword;
    }

}
