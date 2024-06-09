using System;
using System.Collections.Generic;

namespace TelecomShop.DBModels;

public partial class CharInvolvement : BaseEntity
{

    public int? ProductId { get; set; }

    public int? CharId { get; set; }

    public string? DefaultValue { get; set; }

    public string? ListValues { get; set; }

    public virtual Characteristic? Char { get; set; }

    public virtual Product? Product { get; set; }
}
