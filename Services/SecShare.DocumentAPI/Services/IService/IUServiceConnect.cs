using SecShare.Core.BaseClass;

namespace SecShare.DocumentAPI.Services.IService;

public interface IUServiceConnect
{
    Task<ResponseDTO> GetUsersShared(IEnumerable<string> userIds);
}
