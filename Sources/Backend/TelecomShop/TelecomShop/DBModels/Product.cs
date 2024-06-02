using System;
using System.Collections.Generic;

namespace TelecomShop.DBModels;

public partial class Product:BaseEntity
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public int? ParentId { get; set; }

    public DateTime? ActiveFrom { get; set; }

    public DateTime? ActiveTo { get; set; }

    public float? PriceOneTime { get; set; }

    public float? PriceRecurrent { get; set; }

    public float? PriceOneTimeTotal { get; set; }

    public float? PriceRecurrentTotal { get; set; }

    public string? Description { get; set; }

    public string? UpgradeOptions { get; set; }

    public string? DowngradeOptions { get; set; }

    public virtual ICollection<ActiveProduct> ActiveProducts { get; set; } = new List<ActiveProduct>();

    public virtual ICollection<CharInvolvement> CharInvolvements { get; set; } = new List<CharInvolvement>();
}
