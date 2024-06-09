using System;
using System.Collections.Generic;

namespace TelecomShop.DBModels;

public partial class Product : BaseEntity
{

    public string? Name { get; set; }

    public string? Type { get; set; }

    public int? ParentId { get; set; }

    public DateOnly? ActiveFrom { get; set; }

    public DateOnly? ActiveTo { get; set; }

    public float? PriceOneTime { get; set; }

    public float? PriceRecurrent { get; set; }

    public string? Description { get; set; }

    public int Rate { get; set; }

    public virtual ICollection<ActiveProduct> ActiveProducts { get; set; } = new List<ActiveProduct>();

    public virtual ICollection<CharInvolvement> CharInvolvements { get; set; } = new List<CharInvolvement>();
}
