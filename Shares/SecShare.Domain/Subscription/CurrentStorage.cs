using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Subscription;
public class CurrentStorage : BaseClass.BaseClass
{

    public string UserId { get; set; }
    public long UsedSize { get; set; }
    public long StorageLimitMB { get; set; }
}
