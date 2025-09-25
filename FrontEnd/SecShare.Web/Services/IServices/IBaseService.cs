using SecShare.Core.BaseClass;

namespace SecShare.Web.Services.IServices;

public interface IBaseService
{
    Task<ResponseDTO?> SendAsync(RequestDTO requestDto, bool withBearer = true);
}
