namespace NihongoLearning.Models;

public partial class ShopItem
{
    public int ItemId { get; set; }

    public string? ItemName { get; set; }

    public int? Price { get; set; }

    public string? Description { get; set; }

    public string? ItemType { get; set; }

    public string? IconUrl { get; set; }

    public bool? IsActive { get; set; }

    // Navigation property
    public virtual ICollection<UserInventory> UserInventories { get; set; } = new List<UserInventory>();
}