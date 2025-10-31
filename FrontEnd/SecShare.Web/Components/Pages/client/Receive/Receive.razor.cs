using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using SecShare.Core.Dtos;
using SecShare.Services;
using SecShare.Web.Services.IServices;
using System.Globalization;

namespace SecShare.Web.Components.Pages.client.Receive;

public partial class Receive : ComponentBase
{
    [Inject] private IDocumentService _documentService { get; set; } = default!;
    [Inject] private NotificationService _notificationService { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Inject] private IFileService _fileService { get; set; } = default!;

    // Source data from your service
    private List<DocumentDto> listFiles { get; set; } = new();

    // UI state
    private DocumentDto? SelectedFile { get; set; }
    private string searchQuery { get; set; } = string.Empty;
    private string sortOrder { get; set; } = string.Empty;

    private bool isHover = false;
    private bool isLoading = true;
    // Computed filtered list (getter so UI always sees latest state)
    private IEnumerable<DocumentDto> FilteredFiles => ApplyFilterAndSort();

    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
        await LoadFilesAsync();
    }

    private async Task LoadFilesAsync()
    {
        try
        {
            var rs = await _documentService.GetListDocShared();
            listFiles = JsonConvert.DeserializeObject<List<DocumentDto>>(Convert.ToString(rs.Result)!) ?? new List<DocumentDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading listFiles: " + ex.Message);
            listFiles = new List<DocumentDto>();
        }
        finally
        {
            isLoading = false;
        }

        StateHasChanged();
    }

    // Keep your original ViewFileAsync behavior
    private async Task ViewFileAsync(string documentId)
    {
        try
        {
            var response = await _fileService.GetTempViewLinkAsync(documentId);
            if (!string.IsNullOrEmpty(response))
            {
                Console.WriteLine($"👉 Opening: {response}");
                await JS.InvokeVoidAsync("open", $"/fileview/{response.Split("api/")[1]}", "_blank");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening file: {ex.Message}");
        }
    }

    // UI helpers and handlers
    private string GetFileIcon(string fileName)
    {
        var ext = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".docx" => "https://img.icons8.com/color/96/000000/ms-word.png",
            ".pptx" => "https://img.icons8.com/color/96/000000/ms-powerpoint.png",
            ".xlsx" => "https://img.icons8.com/color/96/000000/ms-excel.png",
            ".pdf" => "https://img.icons8.com/color/96/000000/pdf.png",
            ".jpg" or ".jpeg" or ".png" => "https://img.icons8.com/color/96/000000/image.png",
            _ => "https://img.icons8.com/fluency/96/000000/file.png"
        };
    }

    private string FormatDate(DateTime? dt)
    {
        if (dt == null) return string.Empty;
        return dt.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    private void ShowSidebar(DocumentDto file)
    {
        SelectedFile = file;
    }

    private void SelectCard(DocumentDto file)
    {
        SelectedFile = file;
    }

    private void CloseSidebar()
    {
        SelectedFile = null;
    }

    private async Task OnOpenFile(DocumentDto file)
    {
        if (file == null) return;
        await ViewFileAsync(file.Id.ToString());
    }

    private void ApplyFilter()
    {
        // just trigger UI recompute (FilteredFiles getter will apply filter)
        StateHasChanged();
    }

    private void OnSortChange(ChangeEventArgs e)
    {
        sortOrder = e.Value?.ToString() ?? string.Empty;
        StateHasChanged();
    }

    private IEnumerable<DocumentDto> ApplyFilterAndSort()
    {
        var q = (searchQuery ?? string.Empty).Trim();
        IEnumerable<DocumentDto> result = listFiles ?? Enumerable.Empty<DocumentDto>();

        if (!string.IsNullOrEmpty(q))
        {
            result = result.Where(f => (f.FileName ?? string.Empty).Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        if (sortOrder == "asc")
            result = result.OrderBy(f => f.UpdatedAt);
        else if (sortOrder == "desc")
            result = result.OrderByDescending(f => f.UpdatedAt);

        return result;
    }
}
