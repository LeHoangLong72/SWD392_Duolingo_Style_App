using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.DTOs.Item;
using MyWebApiApp.Interfaces;
using MyWebApiApp.Mappers;

namespace MyWebApiApp.Repository
{
    public class ShopRepository : IShopRepository
    {
        private readonly ApplicationDbContext _context;

        public ShopRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ItemResponse>> GetAllItemsAsync(string? category)
        {
            var query = _context.Items.Where(i => i.IsActive);
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(i => i.Category.ToLower() == category.ToLower());
            }
            return await query.Select(i => i.ToItemResponse()).ToListAsync();
        }
    }
}
