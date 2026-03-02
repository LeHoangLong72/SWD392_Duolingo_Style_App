using Microsoft.AspNetCore.Mvc;
using Shop_Duolingo.DTOs;
using Shop_Duolingo.Services;

namespace Shop_Duolingo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        /// <summary>
        /// Lấy danh sách tất cả vật phẩm trong shop
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>Danh sách vật phẩm với trạng thái mua và trang bị</returns>
        /// <response code="200">Trả về danh sách vật phẩm thành công</response>
        [HttpGet("items/{userId}")]
        [ProducesResponseType(typeof(List<ItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ItemDto>>> GetItems(int userId)
        {
            var items = await _shopService.GetAllItemsAsync(userId);
            return Ok(items);
        }

        /// <summary>
        /// Mua vật phẩm từ shop
        /// </summary>
        /// <param name="request">Thông tin mua hàng (UserId và ItemId)</param>
        /// <returns>Kết quả giao dịch mua hàng</returns>
        /// <response code="200">Mua thành công</response>
        /// <response code="400">Mua thất bại (không đủ gems, đã sở hữu, v.v.)</response>
        [HttpPost("purchase")]
        [ProducesResponseType(typeof(PurchaseItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PurchaseItemResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PurchaseItemResponse>> PurchaseItem([FromBody] PurchaseItemRequest request)
        {
            var response = await _shopService.PurchaseItemAsync(request.UserId, request.ItemId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Lấy thông tin profile người dùng và các vật phẩm đã mua
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>Thông tin profile và danh sách vật phẩm đã mua</returns>
        /// <response code="200">Lấy profile thành công</response>
        /// <response code="404">Không tìm thấy người dùng</response>
        [HttpGet("profile/{userId}")]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(int userId)
        {
            var profile = await _shopService.GetUserProfileAsync(userId);

            if (profile.Id == 0)
            {
                return NotFound(new { Message = "Không tìm thấy người dùng" });
            }

            return Ok(profile);
        }

        /// <summary>
        /// Trang bị hoặc gỡ trang bị vật phẩm
        /// </summary>
        /// <param name="request">Thông tin (UserId và ItemId)</param>
        /// <returns>Kết quả trang bị</returns>
        /// <response code="200">Trang bị thành công</response>
        /// <response code="400">Không thể trang bị (chưa sở hữu vật phẩm)</response>
        [HttpPost("equip")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EquipItem([FromBody] PurchaseItemRequest request)
        {
            var success = await _shopService.EquipItemAsync(request.UserId, request.ItemId);

            if (!success)
            {
                return BadRequest(new { Message = "Không thể trang bị vật phẩm này" });
            }

            return Ok(new { Message = "Trang bị thành công" });
        }
    }
}