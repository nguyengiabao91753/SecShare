using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using SecShare.Core.Dtos;
using SecShare.Services;
using SecShare.Web.Services.IServices;

namespace SecShare.Web.Components.Pages.client.Receive;

public partial class Receive
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


    private List<DocumentDto> listFiles { get; set; } = new();


    private bool _firstRender = true;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _firstRender = false;


            var rs = await _documentService.GetListDocShared();
           
            listFiles = JsonConvert.DeserializeObject<List<DocumentDto>>(Convert.ToString(rs.Result)!)!;
            
        


            StateHasChanged(); // cập nhật UI
        }

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
}
