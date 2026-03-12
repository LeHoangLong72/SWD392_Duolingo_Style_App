using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApiApp.Controllers
{
    [Route("api/achievements")]
    [ApiController]
    public class AchievementController : ControllerBase
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

        [HttpPost("{achievementId}/claim")]
        public async Task<IActionResult> ClaimAchievement(int achievementId)
        {
            // TODO: implement claim logic

            return Ok(new
            {
                success = true,
                message = "Achievement reward claimed",
                rewardXP = 50,
                rewardGems = 10
            });
        }
    }
}
