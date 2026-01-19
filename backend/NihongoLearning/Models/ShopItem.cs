using System;
using System.Collections.Generic;

namespace NihongoLearning.Models;

public partial class ShopItem
{
    public int ItemId { get; set; }

    public string? ItemName { get; set; }

    public int? Price { get; set; }

    public string? Description { get; set; }
}
