using System;
using System.Collections.Generic;

namespace TelecomShop.DBModels;

public partial class Characteristic:BaseEntity
{
    public string? Name { get; set; }

    public int Id { get; set; }

    public virtual ICollection<CharInvolvement> CharInvolvements { get; set; } = new List<CharInvolvement>();
}
