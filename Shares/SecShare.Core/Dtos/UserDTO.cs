using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Dtos;
public class UserDto
{
    public string ID { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
    public string Name { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string PhoneNumber { get; set; }


    public long UsedSize { get; set; }

    public long StorageLimitMB { get; set; }
}
