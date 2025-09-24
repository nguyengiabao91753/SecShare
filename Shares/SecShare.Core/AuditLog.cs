using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core;
public class AuditLog
{
    [Key]
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public Guid? DocumentId { get; set; }
    public string Action { get; set; }
    public string DeviceInfo { get; set; }
    public string IpAddress { get; set; }
    public DateTime Timestamp { get; set; }
}
