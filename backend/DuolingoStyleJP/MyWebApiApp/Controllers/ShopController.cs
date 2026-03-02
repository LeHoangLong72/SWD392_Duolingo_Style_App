using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.DTOs.Item;
using MyWebApiApp.DTOs.Purchase;
using MyWebApiApp.DTOs.UserItem;
using MyWebApiApp.Interfaces;
using MyWebApiApp.Models;
using System.Net.ServerSentEvents;
using System.Security.Claims;

namespace MyWebApiApp.Controllers
{
    [Route("api/shop")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopRepository _shopRepo;
        private readonly ApplicationDbContext _context;

        public ShopController(IShopRepository shopRepo, ApplicationDbContext context)
        {
            _shopRepo = shopRepo;
            _context = context;
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetAllItems([FromQuery] string? category)
        {
            var query = _context.Items.Where(i => i.IsActive);
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(i => i.Category.ToLower() == category.ToLower());
            }
            var items = await query
                .Select(i => new ItemResponse
                {
                    ItemId = i.ItemId,
                    Name = i.Name,
                    Description = i.Description,
                    Price = i.Price,
                    Category = i.Category,
                    ImageUrl = i.ImageUrl
                })
                .ToListAsync();
            return Ok(items);
        }

        //[HttpPost("purchase")]
        //[Authorize]
        //public async Task<IActionResult> PurchaseItem([FromBody] PurchaseRequest request)
        //{
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
        //    {
        //        return Unauthorized();
        //    }
        //    var user = await _context.Users.FindAsync(userId);
        //    if (user == null)
        //        return NotFound(new { message = "User not found" });

        //    var item = await _context.Items.FindAsync(request.ItemId);
        //    if (item == null || !item.IsActive)
        //        return NotFound(new { message = "Item not found" });

        //    int totalPrice = item.Price * request.Quantity;

        //    // Check if user has enough currency (assuming you have a Currency property on User)
        //    // if (user.Currency < totalPrice)
        //    //     return BadRequest(new { message = "Insufficient funds" });

        //    // Deduct currency
        //    // user.Currency -= totalPrice;

        //    // Add or update user item
        //    var existingUserItem = await _context.UserItems
        //        .FirstOrDefaultAsync(ui => ui.UserId == userId && ui.ItemId == request.ItemId);

        //    if (existingUserItem != null)
        //    {
        //        existingUserItem.Quantity += request.Quantity;
        //    }
        //    else
        //    {
        //        var userItem = new UserItem
        //        {
        //            UserId = userId,
        //            ItemId = request.ItemId,
        //            Quantity = request.Quantity,
        //            PurchasedAt = DateTime.UtcNow
        //        };
        //        _context.UserItems.Add(userItem);
        //    }

        //    // Create transaction record
        //    var transaction = new Transaction
        //    {
        //        UserId = userId,
        //        ItemId = request.ItemId,
        //        Quantity = request.Quantity,
        //        TotalPrice = totalPrice,
        //        TransactionType = "purchase",
        //        TransactionDate = DateTime.UtcNow
        //    };
        //    _context.Transactions.Add(transaction);

        //    await _context.SaveChangesAsync();

        //    return Ok(new
        //    {
        //        message = "Purchase successful",
        //        item = item.Name,
        //        quantity = request.Quantity,
        //        totalPrice = totalPrice
        //    });
        //}

        //// GET: api/shop/my-items
        //[HttpGet("my-items")]
        //[Authorize]
        //public async Task<IActionResult> GetUserItems()
        //{
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
        //        return Unauthorized();

        //    var userItems = await _context.UserItems
        //        .Where(ui => ui.UserId == userId)
        //        .Include(ui => ui.Item)
        //        .Select(ui => new UserItemResponse
        //        {
        //            UserItemId = ui.UserItemId,
        //            ItemId = ui.ItemId,
        //            ItemName = ui.Item.Name,
        //            Quantity = ui.Quantity,
        //            PurchasedAt = ui.PurchasedAt
        //        })
        //        .ToListAsync();

        //    return Ok(userItems);
        //}

        //// GET: api/shop/transactions
        //[HttpGet("transactions")]
        //[Authorize]
        //public async Task<IActionResult> GetTransactionHistory()
        //{
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
        //        return Unauthorized();

        //    var transactions = await _context.Transactions
        //        .Where(t => t.UserId == userId)
        //        .Include(t => t.Item)
        //        .OrderByDescending(t => t.TransactionDate)
        //        .Select(t => new
        //        {
        //            t.TransactionId,
        //            ItemName = t.Item.Name,
        //            t.Quantity,
        //            t.TotalPrice,
        //            t.TransactionType,
        //            t.TransactionDate
        //        })
        //        .ToListAsync();

        //    return Ok(transactions);
        //}
    }
}
