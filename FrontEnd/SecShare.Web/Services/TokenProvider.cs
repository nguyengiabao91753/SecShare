using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SecShare.Helper.Utils;
using SecShare.Web.Services.IServices;

namespace SecShare.Web.Services;

public class TokenProvider : ITokenProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private const string TokenKey = "authToken";

    public TokenProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public async Task SetTokenAsync(string token)
    {
        await _sessionStorage.SetAsync(TokenKey, token);
    }

    public async Task<string?> GetTokenAsync()
    {
        var result = await _sessionStorage.GetAsync<string>(TokenKey);
        return result.Success ? result.Value : null;
    }

    public async Task ClearTokenAsync()
    {
        await _sessionStorage.DeleteAsync(TokenKey);
    }
}
