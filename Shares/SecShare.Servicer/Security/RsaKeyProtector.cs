using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Servicer.Security;
public class RsaKeyProtector
{
    private readonly IDataProtector _dataProtector;

    public RsaKeyProtector(IDataProtector dataProtector)
    {
        _dataProtector = dataProtector.CreateProtector("SecShare.RsaPrivateKey");
    }

    public string Protect(byte[] privateKey)
    {
        return _dataProtector.Protect(Convert.ToBase64String(privateKey));
    }

    public byte[] Unprotect(string protectedData)
    {
        string raw = _dataProtector.Unprotect(protectedData);
        return Convert.FromBase64String(raw);
    }

}
