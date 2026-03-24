using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Interfaces;

namespace MyWebApiApp.Controllers
{
    [Route("api/leaderboard")]
    [ApiController]
    [Authorize]
    public class LeaderboardController : ControllerBase
    {

        private readonly ILeaderboardRepository _leaderboardRepo;

        public LeaderboardController(ILeaderboardRepository leaderboardRepo)
        {
            _leaderboardRepo = leaderboardRepo;
        }


        [HttpGet]
        public async Task<IActionResult> GetLeaderboard()
        {
            var result = await _leaderboardRepo.GetLeaderboardAsync();
            return Ok(result);
        }

        [HttpGet("weekly-top3")]
        public async Task<IActionResult> GetWeeklyTop3()
        {
            var result = await _leaderboardRepo.GetWeeklyTop3Async();
            return Ok(result);
        }

        [HttpPost("weekly-reward")]
        public async Task<IActionResult> RewardWeekly()
        {
            await _leaderboardRepo.RewardWeeklyTopAsync();

            return Ok(new
            {
                message = "Weekly rewards distributed successfully"
            });
        }
    }
}
