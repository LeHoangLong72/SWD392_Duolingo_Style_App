using System;
using System.Collections.Generic;

namespace Shop_Duolingo.Models;

public partial class Item
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string NameVi { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string DescriptionVi { get; set; } = null!;

    public int Price { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string Category { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<UserItem> UserItems { get; set; } = new List<UserItem>();
}
