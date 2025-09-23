﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Core.Subscription;
public class PaymentTransaction
{
    [Key]
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public Guid SubscriptionPlanId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
