using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Ok("User đã đăng nhập");
            }
            else
            {
                return Unauthorized("Chưa đăng nhập");
            }
        }
    }
}
