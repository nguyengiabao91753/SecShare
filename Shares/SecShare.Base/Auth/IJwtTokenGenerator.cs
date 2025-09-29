using SecShare.Core.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Base.Auth;
public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser user);
}
