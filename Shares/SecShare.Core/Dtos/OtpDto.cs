using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Dtos
{
    public class OtpDto
    {
        public string userEmail { get; set; }
        public string? otp { get; set; }
    }
}
