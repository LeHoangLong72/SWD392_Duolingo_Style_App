using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.DTOs.Item;
using MyWebApiApp.DTOs.UserProfile;
using MyWebApiApp.Interfaces;
using MyWebApiApp.Mappers;
using MyWebApiApp.Models;

namespace MyWebApiApp.Repository
{
    public class ShopRepository : IShopRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPowerupService _powerupService;

        public ShopRepository(ApplicationDbContext context, IPowerupService powerupService)
        {
            _context = context;
            _powerupService = powerupService;
        }

        public async Task<bool> EquipItemAsync(string userId, int itemId)
        {
            var userItem = await _context.UserItems
                 .Include(ui => ui.Item)
                 .FirstOrDefaultAsync(ui => ui.UserId == userId && ui.ItemId == itemId);

            if (userItem == null)
                return false;

            var item = userItem.Item;

            // ======================
            // POWERUP ITEM
            // ======================
            if (item.Category.Equals("powerup", StringComparison.OrdinalIgnoreCase))
            {
                if (item.DurationMinutes == null)
                    return false;

                userItem.ActivatedAt = DateTime.UtcNow;
                userItem.ExpiredAt = DateTime.UtcNow.AddMinutes(item.DurationMinutes.Value);

                _context.UserItems.Remove(userItem);

                await _context.SaveChangesAsync();
                await _powerupService.ActivatePowerupAsync(userId, item.ItemId);
                return true;
            }

            //// ======================
            //// OUTFIT ITEM
            //// ======================
            //if (item.Category.Equals("outfit", StringComparison.OrdinalIgnoreCase))
            //{
            //    var equippedOutfits = await _context.UserItems
            //        .Include(ui => ui.Item)
            //        .Where(ui =>
            //            ui.UserId == userId &&
            //            ui.Item.Category == "outfit" &&
            //            ui.IsEquipped)
            //        .ToListAsync();

            //    foreach (var outfit in equippedOutfits)
            //    {
            //        outfit.IsEquipped = false;
            //    }

            //    userItem.IsEquipped = true;
            //    userItem.IsConsumed = true;

            //    await _context.SaveChangesAsync();
            //    return true;
            //}

            //// ======================
            //// DECORATION ITEM
            //// ======================
            //if (item.Category.Equals("decoration", StringComparison.OrdinalIgnoreCase))
            //{
            //    userItem.IsEquipped = true;

            //    userItem.IsConsumed = true;

            //    await _context.SaveChangesAsync();
            //    return true;
            //}

            return false;
        }

        public async Task<List<ItemDto>> GetAllItemsAsync(string? category)
        {
            var query = _context.Items.Where(i => i.IsActive);
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(i => i.Category.ToLower() == category.ToLower());
            }
            return await query.Select(i => i.ToItemResponse()).ToListAsync();
        }

        public async Task<UserProfileDto> GetUserInventoryAsync(string userId)
        {
            var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new UserProfileDto();

            var items = await _context.UserItems
                .Include(ui => ui.Item)
                .Where(ui => ui.UserId == userId && !ui.IsConsumed)
                .Select(ui => new ItemDto
                {
                    Id = ui.Item.ItemId,
                    Name = ui.Item.Name,
                    Description = ui.Item.Description,
                    Price = ui.Item.Price,
                    ImageUrl = ui.Item.ImageUrl,
                    Category = ui.Item.Category,
                    IsPurchased = true,
                    IsEquipped = ui.IsEquipped
                })
                .ToListAsync();

            return new UserProfileDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Gems = user.Gems,
                PurchasedItems = items
            };
        }

        public async Task<PurchaseItemResponse> PurchaseItemAsync(string userId, int itemId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new PurchaseItemResponse
                {
                    Success = false,
                    Message = "Người dùng không tồn tại"
                };
            }

            var item = await _context.Items.FindAsync(itemId);
            if (item == null || !item.IsActive)
            {
                return new PurchaseItemResponse
                {
                    Success = false,
                    Message = "Vật phẩm không tồn tại hoặc không khả dụng"
                };
            }

            var existingPurchase = await _context.UserItems
                .Include(ui => ui.Item)
                .FirstOrDefaultAsync(ui => 
                    ui.UserId == userId && 
                    ui.ItemId == itemId &&
                    !ui.Item.IsConsumable);

            if (existingPurchase != null)
            {
                return new PurchaseItemResponse
                {
                    Success = false,
                    Message = "Bạn đã sở hữu vật phẩm này rồi",
                    RemainingGems = user.Gems
                };
            }

            if (user.Gems < item.Price)
            {
                return new PurchaseItemResponse
                {
                    Success = false,
                    Message = $"Không đủ gems. Cần {item.Price} gems, bạn có {user.Gems} gems",
                    RemainingGems = user.Gems
                };
            }

            user.Gems -= item.Price;

            var userItem = new UserItem
            {
                UserId = userId,
                ItemId = itemId,
                PurchasedAt = DateTime.UtcNow,
                IsEquipped = false
            };

            _context.UserItems.Add(userItem);
            await _context.SaveChangesAsync();

            return new PurchaseItemResponse
            {
                Success = true,
                Message = $"Mua {item.Name} thành công!",
                RemainingGems = user.Gems
            };
        }

        public async Task UseItemAsync(string userId, int itemId)
        {
            var userItem = await _context.UserItems
        .Include(x => x.Item)
        .FirstOrDefaultAsync(x =>
            x.UserId == userId &&
            x.ItemId == itemId &&
            !x.IsConsumed);

            if (userItem == null)
                throw new Exception("User không có item này");

            if (userItem.Item.DurationMinutes == null)
                throw new Exception("Item này không cần kích hoạt");

            userItem.ActivatedAt = DateTime.UtcNow;
            userItem.ExpiredAt = DateTime.UtcNow.AddMinutes(userItem.Item.DurationMinutes.Value);
            userItem.IsConsumed = true;
            userItem.IsEquipped = false;

            await _context.SaveChangesAsync();
        }
    }
}
