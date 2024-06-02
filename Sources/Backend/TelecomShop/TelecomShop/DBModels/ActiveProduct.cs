using System;
using System.Collections.Generic;

namespace TelecomShop.DBModels;

public partial class ActiveProduct:BaseEntity
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? ProductId { get; set; }

    public int? ParentProductId { get; set; }

    public int? ContractTerm { get; set; }

    public float? DataAmount { get; set; }

    public float? VoiceAmount { get; set; }

    public float? SmsAmount { get; set; }

    public string? ExtendedChars { get; set; }

    public int? BillingAccountId { get; set; }

    public DateTime? PurchaceDate { get; set; }

    public float? DataLeft { get; set; }

    public float? VoiceLeft { get; set; }

    public float? SmsLeft { get; set; }

    public virtual BillingAccount? BillingAccount { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User User { get; set; } = null!;
}
