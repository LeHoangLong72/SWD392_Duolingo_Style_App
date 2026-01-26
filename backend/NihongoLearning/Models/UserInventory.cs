namespace NihongoLearning.Models;

public partial class UserInventory
{
    public int InventoryId { get; set; }

    public int UserId { get; set; }

    public int ItemId { get; set; }

    public int Quantity { get; set; }

    public DateTime? PurchasedDate { get; set; }

    public bool? IsUsed { get; set; }

    public DateTime? UsedDate { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;

    public virtual ShopItem Item { get; set; } = null!;
}