using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Helper.EmailHelper
{
    public interface IEmailHelper
    {
        public Task<bool> SendEmailAsync(string toEmail, string otp);
    }
}
