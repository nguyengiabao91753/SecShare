using SecShare.Helper.Utils;
using SecShare.Web.Services.IServices;

namespace SecShare.Web.Services;

public class TokenProvider : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    public TokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _contextAccessor = httpContextAccessor;
    }
    public void ClearToken()
    {
        _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.TokenCookie);
    }

    public string? GetToken()
    {
        string token = null;
        bool? hashToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TokenCookie, out token);

        return hashToken is true ? token : null;
    }

    public void SetToken(string token)
    {
        _contextAccessor.HttpContext?.Response.Cookies.Append(SD.TokenCookie, token);
    }
}
