namespace SecShare.Web.Services.IServices;

public interface ITokenProvider
{
    Task SetTokenAsync(string token);
    Task<string?> GetTokenAsync();
    Task ClearTokenAsync();
}
