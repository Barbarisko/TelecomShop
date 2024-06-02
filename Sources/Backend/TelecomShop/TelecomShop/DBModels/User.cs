﻿using System;
using System.Collections.Generic;

namespace TelecomShop.DBModels;

public partial class User:BaseEntity
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string Msisdn { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<ActiveProduct> ActiveProducts { get; set; } = new List<ActiveProduct>();

    public virtual ICollection<BillingAccount> BillingAccounts { get; set; } = new List<BillingAccount>();
}
