using Microsoft.EntityFrameworkCore;
using Shop_Duolingo.DTOs;
using Shop_Duolingo.Models;

namespace Shop_Duolingo.Services
{
    public class ShopService : IShopService
    {
        private readonly JapaneseLearningShopContext _context;

        public ShopService(JapaneseLearningShopContext context)
        {
            _context = context;
        }

        public async Task<List<ItemDto>> GetAllItemsAsync(int userId)
        {
            var items = await _context.Items
                .Where(i => i.IsActive)
                .ToListAsync();

            var userItems = await _context.UserItems
                .Where(ui => ui.UserId == userId)
                .ToListAsync();

            return items.Select(item =>
            {
                var userItem = userItems.FirstOrDefault(ui => ui.ItemId == item.Id);
                return new ItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    NameVi = item.NameVi,
                    Description = item.Description,
                    DescriptionVi = item.DescriptionVi,
                    Price = item.Price,
                    ImageUrl = item.ImageUrl,
                    Category = item.Category,
                    IsPurchased = userItem != null,
                    IsEquipped = userItem?.IsEquipped ?? false
                };
            }).ToList();
        }

        public async Task<PurchaseItemResponse> PurchaseItemAsync(int userId, int itemId)
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
                .FirstOrDefaultAsync(ui => ui.UserId == userId && ui.ItemId == itemId);

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
                Message = $"Mua {item.NameVi} thành công!",
                RemainingGems = user.Gems
            };
        }

        public async Task<UserProfileDto> GetUserProfileAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserItems)
                .ThenInclude(ui => ui.Item)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new UserProfileDto();
            }

            return new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Gems = user.Gems,
                PurchasedItems = user.UserItems.Select(ui => new ItemDto
                {
                    Id = ui.Item.Id,
                    Name = ui.Item.Name,
                    NameVi = ui.Item.NameVi,
                    Description = ui.Item.Description,
                    DescriptionVi = ui.Item.DescriptionVi,
                    Price = ui.Item.Price,
                    ImageUrl = ui.Item.ImageUrl,
                    Category = ui.Item.Category,
                    IsPurchased = true,
                    IsEquipped = ui.IsEquipped
                }).ToList()
            };
        }

        public async Task<bool> EquipItemAsync(int userId, int itemId)
        {
            var userItem = await _context.UserItems
                .Include(ui => ui.Item)
                .FirstOrDefaultAsync(ui => ui.UserId == userId && ui.ItemId == itemId);

            if (userItem == null)
            {
                return false;
            }

            // PHÂN LOẠI: Powerup (consumable) vs Outfit/Decoration
            if (userItem.Item.Category == "powerup")
            {
                // POWERUP: Sử dụng 1 lần → XÓA KHỎI INVENTORY
                _context.UserItems.Remove(userItem);
                await _context.SaveChangesAsync();
                return true;
            }
            else if (userItem.Item.Category == "outfit")
            {
                // OUTFIT: Chỉ trang bị 1 outfit cùng lúc
                var equippedOutfits = await _context.UserItems
                    .Include(ui => ui.Item)
                    .Where(ui => ui.UserId == userId && ui.Item.Category == "outfit" && ui.IsEquipped)
                    .ToListAsync();

                foreach (var outfit in equippedOutfits)
                {
                    outfit.IsEquipped = false;
                }

                userItem.IsEquipped = !userItem.IsEquipped;
                await _context.SaveChangesAsync();
                return true;
            }
            else if (userItem.Item.Category == "decoration")
            {
                // DECORATION: Toggle on/off (không giới hạn số lượng)
                userItem.IsEquipped = !userItem.IsEquipped;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}