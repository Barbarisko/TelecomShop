using System;
using System.Collections.Generic;

namespace TelecomShop.DBModels;

public partial class MonthlyUsage : BaseEntity
{
    public int? UserId { get; set; }

    public DateOnly? DateStart { get; set; }

    public DateOnly? DateEnd { get; set; }

    public int? DataUsed { get; set; }

    public int? VoiceUsed { get; set; }

    public int? SmsUsed { get; set; }

    public float? MoneySpent { get; set; }

    public int? ActiveProductId { get; set; }

    public virtual ActiveProduct? ActiveProduct { get; set; }

    public virtual User? User { get; set; }
}
