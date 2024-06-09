using System;
using System.Collections.Generic;

namespace TelecomShop.DBModels;

public partial class ActiveProduct : BaseEntity
{
    

    public int UserId { get; set; }

    public int? ProductId { get; set; }

    public int? ParentProductId { get; set; }

    public int? ContractTerm { get; set; }

    public float? DataAmount { get; set; }

    public float? VoiceAmount { get; set; }

    public float? SmsAmount { get; set; }

    public string? ExtendedChars { get; set; }

    public int? BillingAccountId { get; set; }

    public float? DataLeft { get; set; }

    public float? VoiceLeft { get; set; }

    public float? SmsLeft { get; set; }

    public float? OneTimeTotal { get; set; }

    public float? RecurrentTotal { get; set; }

    public string? Status { get; set; }

    public DateOnly? PurchaceDate { get; set; }

    public virtual BillingAccount? BillingAccount { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ICollection<Usage> Usages { get; set; } = new List<Usage>();

    public virtual User User { get; set; } = null!;
}
