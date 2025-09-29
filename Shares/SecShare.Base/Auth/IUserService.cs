using SecShare.Core.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Base.Auth
{
    public interface IUserService
    {
        Task<ResponseDTO> getUserInfor(string userid);
    }
}
