using System;
using System.Collections.Generic;

namespace Shop_Duolingo.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Gems { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<UserItem> UserItems { get; set; } = new List<UserItem>();
}
