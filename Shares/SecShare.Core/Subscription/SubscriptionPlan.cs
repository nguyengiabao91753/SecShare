using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Subscription;
public class SubscriptionPlan : BaseClass.BaseClass
{

    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public long StorageLimitMB { get; set; }
    public int DurationInDays { get; set; }
}
