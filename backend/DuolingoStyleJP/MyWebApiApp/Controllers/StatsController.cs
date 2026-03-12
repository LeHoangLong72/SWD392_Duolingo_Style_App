using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApiApp.Controllers
{
    [Route("api/stats")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        /// <summary>
        /// Lấy danh sách achievement của user
        /// </summary>
        [HttpGet]
        public IActionResult GetAchievements()
        {
            // TODO: implement logic

            return Ok(new
            {
                message = "Get achievements endpoint created"
            });
        }

        /// <summary>
        /// Claim achievement
        /// </summary>
        [HttpPost("claim")]
        public IActionResult ClaimAchievement()
        {
            // TODO: implement logic

            return Ok(new
            {
                message = "Claim achievement endpoint created"
            });
        }
    }
}
