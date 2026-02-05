using MyWebApiApp.DTOs.Item;

namespace MyWebApiApp.Interfaces
{
    public interface IShopRepository
    {
        Task<List<ItemResponse>> GetAllItemsAsync(string? category);
    }
}
