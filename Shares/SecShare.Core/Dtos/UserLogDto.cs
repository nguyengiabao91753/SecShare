using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Dtos;
public class UserLogDto
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; }
    public Guid? DocumentId { get; set; }
    public string Action { get; set; }
    public string DeviceInfo { get; set; }
    public string IpAddress { get; set; }
    public DateTime Timestamp { get; set; }
}
