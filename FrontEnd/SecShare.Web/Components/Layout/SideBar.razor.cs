using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using SecShare.Core.Dtos;
using SecShare.Web.Services.IServices;
using SecShare.Web.Services.IServices.IUserServices;

namespace SecShare.Web.Components.Layout;

public partial class SideBar
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }
    [Inject]
    private ITokenProvider _tokenProvider { get; set; }

    [Inject]
    private IUserService _userService { get; set; }
    [SupplyParameterFromForm]
    private UserDto? userModel { get; set; } = new UserDto();

    private bool _firstRender = true;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _firstRender = false;

            var rs = await _userService.getUserInfor(); // gọi API an toàn
            userModel = JsonConvert.DeserializeObject<UserDto>(Convert.ToString(rs.Result));


            StateHasChanged(); // cập nhật UI
        }

    }

    private async Task Logout()
    {
        await _tokenProvider.ClearTokenAsync();
        _navigationManager.NavigateTo("/login");
    }
}
