using System;
using System.Collections.Generic;

namespace Shop_Duolingo.Models;

public partial class UserItem
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ItemId { get; set; }

    public DateTime PurchasedAt { get; set; }

    public bool IsEquipped { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
