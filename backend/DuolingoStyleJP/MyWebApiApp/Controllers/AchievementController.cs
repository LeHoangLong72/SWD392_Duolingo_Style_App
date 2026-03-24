using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Interfaces;
using MyWebApiApp.Repository;
using System.Security.Claims;

namespace MyWebApiApp.Controllers
{
    [Route("api/achievements")]
    [ApiController]
    [Authorize]
    public class AchievementController : ControllerBase
    {
        private readonly IAchievementRepository _achievementRepo;

        public AchievementController(IAchievementRepository achievementRepo)
        {
            _achievementRepo = achievementRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAchievements()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var achievements = await _achievementRepo.GetAllAchievementsAsync();
            var userAchievements = await _achievementRepo.GetUserAchievementsAsync(userId);

            var result = achievements.Select(a => new
            {
                a.AchievementId,
                a.Name,
                a.Description,
                a.IconUrl,
                a.RequiredValue,
                a.AchievementType,
                unlocked = userAchievements.Any(ua => ua.AchievementId == a.AchievementId)
            });

            return Ok(result);
        }

        [HttpPost("{achievementId}/claim")]
        public async Task<IActionResult> ClaimAchievement(int achievementId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userAchievement = await _achievementRepo
                .GetUserAchievementAsync(userId, achievementId);

            if (userAchievement == null)
            {
                return BadRequest("Thành tựu chưa mở khóa!");
            }

            // TODO: add reward logic
            int rewardXP = 50;
            int rewardGems = 10;

            return Ok(new
            {
                success = true,
                message = "Phần thưởng thành tựu đã được nhận!",
                rewardXP,
                rewardGems
            });
        }
    }
}
