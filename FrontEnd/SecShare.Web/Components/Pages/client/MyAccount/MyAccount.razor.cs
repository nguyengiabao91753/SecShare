using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SecShare.Core.Dtos;
using Newtonsoft.Json;
using SecShare.Web.Services.IServices.IUserServices;

namespace SecShare.Web.Components.Pages.client.MyAccount
{
    public partial class MyAccount
    {
        [Inject]
        private IUserService _userService { get; set; }


        [SupplyParameterFromForm]
        private UserDto? userModel { get; set; } = new UserDto();
        private ChangePasswordDto? changePasswordModel { get; set; } = new ChangePasswordDto();
        private string errorMessage = "";
        private bool isSubmitting = false;

        private string successMessage;
        private bool showSuccess;
        bool showPasswordOld = false;
        bool showPasswordNew = false;
        bool showPasswordConfirm = false;
        private EditContext editContext;
        private string activeTab = "personal";

        private bool _firstRender = true;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _firstRender = false;

                var rs = await _userService.getUserInfor(); // gọi API an toàn
                userModel = JsonConvert.DeserializeObject<UserDto>(Convert.ToString(rs.Result));
                editContext = new EditContext(userModel);


                StateHasChanged(); // cập nhật UI
            }

        }

        private void SetActiveTab(string tab)
        {
            activeTab = tab;
        }

        //đổi pass
        protected override void OnInitialized()
        {
            editContext = new EditContext(changePasswordModel);
        }

        //Cap nhat thong tin user
        private async Task HandleUpdate()
        {
            isSubmitting = true;
            try
            {
                var result = await _userService.updateInformation(userModel);
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
                successMessage = result.Message;
                showSuccess = true;
                StateHasChanged();

                // Chờ 3 giây rồi redirect
                await Task.Delay(3000);
                Navigation.NavigateTo("/myspace", forceLoad: true);
            }
            finally
            {
                isSubmitting = false;

            }
        }

        private async Task HandleChangePassword()
        {
            isSubmitting = true;
            try
            {
                var result = await _userService.changePassword(changePasswordModel);
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
                successMessage = result.Message;
                showSuccess = true;
                StateHasChanged();

                // Chờ 3 giây rồi redirect
                await Task.Delay(3000);
                Navigation.NavigateTo("/Login");
            }
            finally
            {
                isSubmitting = false;

            }
        }


        void TogglePassword(string field)
        {
            switch (field)
            {
                case nameof(changePasswordModel.OldPassword):
                    showPasswordOld = !showPasswordOld;
                    break;
                case nameof(changePasswordModel.NewPassword):
                    showPasswordNew = !showPasswordNew;
                    break;
                case nameof(changePasswordModel.ConfirmPassword):
                    showPasswordConfirm = !showPasswordConfirm;
                    break;
            }
        }



    }
}
