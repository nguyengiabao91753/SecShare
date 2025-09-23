using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Subscription;
public class CurrentStorage
{
    [Key]
    public Guid StorageId { get; set; }
    public string UserId { get; set; }
    public long UsedSize { get; set; }
    public long StorageLimitMB { get; set; }
    public DateTime UpdatedAt { get; set; }
}
