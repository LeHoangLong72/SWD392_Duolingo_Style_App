using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NihongoLearning.DTOs;
using NihongoLearning.Models;

namespace NihongoLearning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<ShopController> _logger;

    public ShopController(AppDbContext context, ILogger<ShopController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách tất cả item trong shop
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShopItemDto>>> GetAllShopItems([FromQuery] int? userId = null)
    {
        try
        {
            var items = await _context.ShopItems
                .Where(i => i.IsActive == true)
                .OrderBy(i => i.Price)
                .ToListAsync();

            // Tối ưu: Lấy tất cả inventory 1 lần thay vì query từng item
            Dictionary<int, int>? userInventory = null;

            if (userId.HasValue)
            {
                userInventory = await _context.UserInventories
                    .Where(ui => ui.UserId == userId.Value)
                    .GroupBy(ui => ui.ItemId)
                    .Select(g => new { ItemId = g.Key, Quantity = g.Sum(x => x.Quantity) })
                    .ToDictionaryAsync(x => x.ItemId, x => x.Quantity);
            }

            var result = items.Select(item => new ShopItemDto
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName ?? "",
                Price = item.Price ?? 0,
                Description = item.Description,
                ItemType = item.ItemType,
                IconUrl = item.IconUrl,
                IsOwned = userInventory?.ContainsKey(item.ItemId) ?? false,
                OwnedQuantity = userInventory?.GetValueOrDefault(item.ItemId, 0) ?? 0
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách shop items");
            return StatusCode(500, new { message = "Có lỗi xảy ra khi tải danh sách shop" });
        }
    }

    /// <summary>
    /// Lấy chi tiết 1 item trong shop
    /// </summary>
    [HttpGet("{itemId}")]
    public async Task<ActionResult<ShopItemDto>> GetItemById(int itemId, [FromQuery] int? userId = null)
    {
        try
        {
            var item = await _context.ShopItems.FindAsync(itemId);

            if (item == null || item.IsActive != true)
            {
                return NotFound(new { message = "Không tìm thấy vật phẩm" });
            }

            var owned = 0;
            var isOwned = false;

            if (userId.HasValue)
            {
                owned = await _context.UserInventories
                    .Where(ui => ui.UserId == userId.Value && ui.ItemId == itemId)
                    .SumAsync(ui => ui.Quantity);
                isOwned = owned > 0;
            }

            return Ok(new ShopItemDto
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName ?? "",
                Price = item.Price ?? 0,
                Description = item.Description,
                ItemType = item.ItemType,
                IconUrl = item.IconUrl,
                IsOwned = isOwned,
                OwnedQuantity = owned
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thông tin item {ItemId}", itemId);
            return StatusCode(500, new { message = "Có lỗi xảy ra" });
        }
    }

    /// <summary>
    /// Mua vật phẩm
    /// </summary>
    [HttpPost("purchase")]
    public async Task<ActionResult<PurchaseResponse>> PurchaseItem([FromBody] PurchaseItemRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Validate user
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new PurchaseResponse
                {
                    Success = false,
                    Message = "Không tìm thấy user"
                });
            }

            // Validate item
            var item = await _context.ShopItems.FindAsync(request.ItemId);
            if (item == null || item.IsActive != true)
            {
                return NotFound(new PurchaseResponse
                {
                    Success = false,
                    Message = "Vật phẩm không tồn tại hoặc đã ngừng bán"
                });
            }

            // Validate quantity
            if (request.Quantity <= 0)
            {
                return BadRequest(new PurchaseResponse
                {
                    Success = false,
                    Message = "Số lượng phải lớn hơn 0"
                });
            }

            // Tính tổng giá
            int totalPrice = (item.Price ?? 0) * request.Quantity;

            // Kiểm tra đủ gems không
            if ((user.Gems ?? 0) < totalPrice)
            {
                return BadRequest(new PurchaseResponse
                {
                    Success = false,
                    Message = $"Không đủ gems! Cần {totalPrice} gems, bạn chỉ có {user.Gems ?? 0} gems",
                    RemainingGems = user.Gems ?? 0
                });
            }

            // Trừ gems
            user.Gems = (user.Gems ?? 0) - totalPrice;

            // Kiểm tra đã có item trong inventory chưa
            var existingInventory = await _context.UserInventories
                .FirstOrDefaultAsync(ui => ui.UserId == request.UserId
                    && ui.ItemId == request.ItemId
                    && ui.IsUsed == false);

            UserInventory inventory;

            if (existingInventory != null)
            {
                // Tăng số lượng
                existingInventory.Quantity += request.Quantity;
                inventory = existingInventory;
            }
            else
            {
                // Tạo mới
                inventory = new UserInventory
                {
                    UserId = request.UserId,
                    ItemId = request.ItemId,
                    Quantity = request.Quantity,
                    PurchasedDate = DateTime.Now,
                    IsUsed = false
                };
                _context.UserInventories.Add(inventory);
            }

            // Chỉ save 1 lần duy nhất
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var purchasedItem = new InventoryItemDto
            {
                InventoryId = inventory.InventoryId,
                ItemId = item.ItemId,
                ItemName = item.ItemName ?? "",
                ItemType = item.ItemType,
                IconUrl = item.IconUrl,
                Quantity = inventory.Quantity,
                IsUsed = false,
                PurchasedDate = inventory.PurchasedDate ?? DateTime.Now
            };

            _logger.LogInformation("User {UserId} mua {Quantity}x {ItemName} với giá {Price} gems",
                request.UserId, request.Quantity, item.ItemName, totalPrice);

            return Ok(new PurchaseResponse
            {
                Success = true,
                Message = $"Đã mua thành công {request.Quantity}x {item.ItemName}!",
                RemainingGems = user.Gems ?? 0,
                PurchasedItem = purchasedItem
            });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            _logger.LogError(ex, "Lỗi khi user {UserId} mua item {ItemId}",
                request.UserId, request.ItemId);

            return StatusCode(500, new PurchaseResponse
            {
                Success = false,
                Message = "Có lỗi xảy ra khi mua vật phẩm, vui lòng thử lại"
            });
        }
    }

    /// <summary>
    /// Lấy inventory của user
    /// </summary>
    [HttpGet("inventory/{userId}")]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetUserInventory(int userId)
    {
        try
        {
            // Kiểm tra user tồn tại
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists)
            {
                return NotFound(new { message = "Không tìm thấy user" });
            }

            var inventory = await _context.UserInventories
                .Include(ui => ui.Item)
                .Where(ui => ui.UserId == userId && ui.Quantity > 0)
                .OrderByDescending(ui => ui.PurchasedDate)
                .Select(ui => new InventoryItemDto
                {
                    InventoryId = ui.InventoryId,
                    ItemId = ui.ItemId,
                    ItemName = ui.Item.ItemName ?? "",
                    ItemType = ui.Item.ItemType,
                    IconUrl = ui.Item.IconUrl,
                    Quantity = ui.Quantity,
                    IsUsed = ui.IsUsed ?? false,
                    PurchasedDate = ui.PurchasedDate ?? DateTime.Now,
                    UsedDate = ui.UsedDate
                })
                .ToListAsync();

            return Ok(inventory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy inventory của user {UserId}", userId);
            return StatusCode(500, new { message = "Có lỗi xảy ra khi tải túi đồ" });
        }
    }

    /// <summary>
    /// Sử dụng vật phẩm
    /// </summary>
    [HttpPost("use-item")]
    public async Task<ActionResult<UseItemResponse>> UseItem([FromBody] UseItemRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var inventory = await _context.UserInventories
                .Include(ui => ui.Item)
                .Include(ui => ui.User)
                .FirstOrDefaultAsync(ui => ui.InventoryId == request.InventoryId
                    && ui.UserId == request.UserId);

            if (inventory == null)
            {
                return NotFound(new UseItemResponse
                {
                    Success = false,
                    Message = "Không tìm thấy vật phẩm trong túi đồ"
                });
            }

            if (inventory.Quantity <= 0)
            {
                return BadRequest(new UseItemResponse
                {
                    Success = false,
                    Message = "Vật phẩm đã hết"
                });
            }

            // Logic xử lý theo loại item
            string effect = "";
            switch (inventory.Item.ItemType?.ToLower())
            {
                case "streak_freeze":
                    // Logic bảo vệ streak (implement sau)
                    effect = "Streak của bạn được bảo vệ trong 1 ngày!";
                    break;

                case "xp_boost":
                    // Logic tăng XP (implement sau)
                    effect = "XP nhận được tăng 2x trong 1 giờ!";
                    break;

                case "gem_boost":
                    // Logic tăng gems (implement sau)
                    effect = "Gems nhận được tăng 2x trong 1 giờ!";
                    break;

                default:
                    effect = $"Đã sử dụng {inventory.Item.ItemName}";
                    break;
            }

            // Giảm số lượng
            inventory.Quantity -= 1;

            if (inventory.Quantity == 0)
            {
                inventory.IsUsed = true;
                inventory.UsedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("User {UserId} sử dụng item {ItemName}",
                request.UserId, inventory.Item.ItemName);

            return Ok(new UseItemResponse
            {
                Success = true,
                Message = "Sử dụng vật phẩm thành công!",
                Effect = effect
            });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            _logger.LogError(ex, "Lỗi khi user {UserId} sử dụng item", request.UserId);

            return StatusCode(500, new UseItemResponse
            {
                Success = false,
                Message = "Có lỗi xảy ra, vui lòng thử lại"
            });
        }
    }
}