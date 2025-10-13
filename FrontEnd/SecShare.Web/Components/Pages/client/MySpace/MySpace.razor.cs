using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using SecShare.Core.Dtos;
using SecShare.Helper.Utils;
using SecShare.Services;
using SecShare.Web.Components.Layout;
using SecShare.Web.Helpers;
using SecShare.Web.Services;
using SecShare.Web.Services.IServices;
using System.Drawing;

namespace SecShare.Web.Components.Pages.client.MySpace;

public partial class MySpace
{
    [Inject]
    private NavigationManager _navigationManager { get; set; }
    [Inject]
    private IDocumentService _documentService { get; set; }
    [Inject]
    private NotificationService _notificationService { get; set; }
    [Inject]
    private IJSRuntime JS { get; set; } = default!;
    [Inject]
    private IFileService _fileService { get; set; }
    [SupplyParameterFromForm]
    private UploadMyFileDto uploadMyFileDto { get; set; }
    [SupplyParameterFromForm]
    private ShareFileDto shareFileDto { get; set; } = new();
    [SupplyParameterFromForm]
    private List<DocumentDto> listFiles { get; set; } = new();
    [SupplyParameterFromForm]
    private List<UserDto> listUsers { get; set; } = new();

    private EditContext? editContext;
    private EditContext? editContextShare;
    private bool isSubmit = false;
    private bool isModalOpen = false; // State để toggle modal

    private bool isUpload = false;
    private bool isShare = false;

    private bool isSidebarOpen = false;
    private DocumentDto? selectedFile;
    private bool _isDisposed = false;
    private bool _firstRender = true;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _firstRender = false;

            uploadMyFileDto = new();
            editContext = new EditContext(uploadMyFileDto);
            editContextShare = new EditContext(shareFileDto);

            var rs = await _documentService.ListFiles();
            listFiles = JsonConvert.DeserializeObject<List<DocumentDto>>(Convert.ToString(rs.Result));

            

            StateHasChanged(); // cập nhật UI
        }

    }

    private void ToggleModal()
    {
        isModalOpen = !isModalOpen;
        if (!isModalOpen)
        {
            ResetForm();
        }
    }

    private void ResetForm()
    {
        uploadMyFileDto = new();
        
        editContext = new EditContext(uploadMyFileDto);
        isUpload = false; // Reset biến isUpload
        StateHasChanged();
    }

    private async Task OnFileSelected(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null && file.Size > 0)
        {
            var fileName = Path.GetFileNameWithoutExtension(file.Name);
            var extension = Path.GetExtension(file.Name).TrimStart('.');

            //var stream = file.OpenReadStream();
            //string content = await FileConverter.C(stream);
            var fileAfterConvert = await FileHelper.ConvertIBrowserFileToIFormFile(file);
            if (fileAfterConvert == null)
            {
                await _notificationService.ShowError("File conversion failed. Please try again.");
                return;
            }
            uploadMyFileDto.File = fileAfterConvert;
            uploadMyFileDto.FileName = fileName; // Tên có thể edit sau
            uploadMyFileDto.Type = extension.ToLower();
            uploadMyFileDto.FileSize = file.Size;

            isUpload = true;
            StateHasChanged();
        }
    }

    private string GetReadableSize(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        double kb = bytes / 1024.0;
        if (kb < 1024) return $"{kb:F2} KB";
        double mb = kb / 1024.0;
        return $"{mb:F2} MB";
    }

    private async Task UploadFile()
    {
        if (uploadMyFileDto.File == null)
        {
            await _notificationService.ShowError("Please select a file first!");
            return;
        }

        // Validation cơ bản: Kiểm tra tên file không rỗng
        if (string.IsNullOrWhiteSpace(uploadMyFileDto.FileName))
        {
            await _notificationService.ShowError("File name cannot be empty!");
            return;
        }

        var rs = await _documentService.UploadMyFile(uploadMyFileDto);
        if (!rs.IsSuccess)
        {
            await _notificationService.ShowError(rs.Message);
            return;
        }
        await _notificationService.ShowSuccess("Upload successful!");
        ResetForm();
        isModalOpen = false; // Đóng modal sau khi upload thành công
    }

    private async Task ShareFile()
    {
        if (selectedFile == null)
        {
            await _notificationService.ShowError("No file selected to share.");
            return;
        }
        // Validation cơ bản: Kiểm tra email không rỗng và định dạng email
        if (string.IsNullOrWhiteSpace(shareFileDto.ReceiverEmail))
        {
            await _notificationService.ShowError("Please enter a valid email address.");
            return;
        }
        shareFileDto.DocumentId = selectedFile.Id;
        shareFileDto.Permissions = SD.FileAccess.All.ToString();
        var rs = await _documentService.ShareFile(shareFileDto);
        if (!rs.IsSuccess)
        {
            await _notificationService.ShowError(rs.Message);
            return;
        }
        await _notificationService.ShowSuccess("File shared successfully!");
        shareFileDto = new ShareFileDto(); // Reset form
        editContextShare = new EditContext(shareFileDto);
        isShare = false;
       
    }

    private void CloseModal()
    {
        isModalOpen = false;
        ResetForm();
        StateHasChanged();
    }


    /// <summary>
    /// Mở Sidebar và load chi tiết file
    /// </summary>
    private async Task OpenSidebar(DocumentDto file)
    {
        selectedFile = file;
        isSidebarOpen = true;
        await LoadUserShare();
    }

    /// <summary>
    /// Đóng Sidebar
    /// </summary>
    private void CloseSidebar()
    {
        isSidebarOpen = false;
        selectedFile = null;
    }

    private async Task ViewFileAsync(string documentId)
    {
        try
        {
            var response = await _fileService.GetTempViewLinkAsync(documentId);
            if (!string.IsNullOrEmpty(response))
            {

                Console.WriteLine($"👉 Opening: {response}");
                await JS.InvokeVoidAsync("open", $"/fileview/{response.Split("api/")[1]}", "_blank");

                //await JS.InvokeVoidAsync("eval", $"window.open('{response}', '_blank');");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening file: {ex.Message}");
        }
    }
    public void Dispose() => _isDisposed = true;


    private async Task LoadUserShare()
    {
        var us = await _documentService.GetListUsersShared(selectedFile.Id.ToString());
        listUsers = JsonConvert.DeserializeObject<List<UserDto>>(Convert.ToString(us.Result));
    }
}