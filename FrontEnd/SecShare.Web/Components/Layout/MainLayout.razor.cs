using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using SecShare.Core.Dtos;
using SecShare.Web.Services.IServices.IUserServices;
using SecShare.Web.Services.IServices;

namespace SecShare.Web.Components.Layout;

public partial class MainLayout
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }
    [Inject]
    private ITokenProvider _tokenProvider { get; set; }

   

    private bool _firstRender = true;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _firstRender = false;

            var token = await _tokenProvider.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                _navigationManager.NavigateTo("/login");
            }
        }

    }
}
