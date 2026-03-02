using Shop_Duolingo.DTOs;

namespace Shop_Duolingo.Services
{
    public interface IShopService
    {
        Task<List<ItemDto>> GetAllItemsAsync(int userId);
        Task<PurchaseItemResponse> PurchaseItemAsync(int userId, int itemId);
        Task<UserProfileDto> GetUserProfileAsync(int userId);
        Task<bool> EquipItemAsync(int userId, int itemId);
    }
}