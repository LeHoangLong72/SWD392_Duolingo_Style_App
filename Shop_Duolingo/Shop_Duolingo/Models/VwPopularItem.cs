using System;
using System.Collections.Generic;

namespace Shop_Duolingo.Models;

public partial class VwPopularItem
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string NameVi { get; set; } = null!;

    public string Category { get; set; } = null!;

    public int Price { get; set; }

    public int? PurchaseCount { get; set; }

    public int? EquippedCount { get; set; }
}
