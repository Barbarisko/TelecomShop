using System;
using System.Collections.Generic;

namespace TelecomShop.DBModels;

public partial class BillingAccount : BaseEntity
{

    public int? UserId { get; set; }

    public string? Type { get; set; }

    public float? Balance { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<ActiveProduct> ActiveProducts { get; set; } = new List<ActiveProduct>();

    public virtual User? User { get; set; }
}
