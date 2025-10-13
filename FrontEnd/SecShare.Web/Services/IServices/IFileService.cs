namespace SecShare.Web.Services.IServices;

public interface IFileService
{
    public Task<string?> GetTempViewLinkAsync(string documentId);
}
