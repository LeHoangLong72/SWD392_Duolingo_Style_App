using System.ComponentModel.DataAnnotations;

namespace NihongoLearning.DTOs;

// Request DTOs
public class PurchaseItemRequest
{
    [Required(ErrorMessage = "UserId là bắt buộc")]
    [Range(1, int.MaxValue, ErrorMessage = "UserId phải lớn hơn 0")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "ItemId là bắt buộc")]
    [Range(1, int.MaxValue, ErrorMessage = "ItemId phải lớn hơn 0")]
    public int ItemId { get; set; }

    [Required(ErrorMessage = "Quantity là bắt buộc")]
    [Range(1, 99, ErrorMessage = "Số lượng phải từ 1 đến 99")]
    public int Quantity { get; set; } = 1;
}

public class UseItemRequest
{
    [Required(ErrorMessage = "UserId là bắt buộc")]
    [Range(1, int.MaxValue, ErrorMessage = "UserId phải lớn hơn 0")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "InventoryId là bắt buộc")]
    [Range(1, int.MaxValue, ErrorMessage = "InventoryId phải lớn hơn 0")]
    public int InventoryId { get; set; }
}

// Response DTOs
public class ShopItemDto
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = null!;
    public int Price { get; set; }
    public string? Description { get; set; }
    public string? ItemType { get; set; }
    public string? IconUrl { get; set; }
    public bool IsOwned { get; set; }
    public int OwnedQuantity { get; set; }
}

public class InventoryItemDto
{
    public int InventoryId { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; } = null!;
    public string? ItemType { get; set; }
    public string? IconUrl { get; set; }
    public int Quantity { get; set; }
    public bool IsUsed { get; set; }
    public DateTime PurchasedDate { get; set; }
    public DateTime? UsedDate { get; set; }
}

public class PurchaseResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public int RemainingGems { get; set; }
    public InventoryItemDto? PurchasedItem { get; set; }
}

public class UseItemResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public string? Effect { get; set; }
}