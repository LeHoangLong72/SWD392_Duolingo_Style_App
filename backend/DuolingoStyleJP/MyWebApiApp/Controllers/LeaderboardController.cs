using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApiApp.Controllers
{
    [Route("api/leaderboard")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        /// <summary>
        /// Lấy bảng xếp hạng XP
        /// </summary>
        [HttpGet]
        public IActionResult GetLeaderboard()
        {
            // TODO: implement leaderboard logic

            return Ok(new
            {
                message = "Leaderboard endpoint created"
            });
        }

        [HttpGet("weekly")]
        public IActionResult GetWeeklyLeaderboard()
        {
            // TODO: implement weekly leaderboard logic

            return Ok(new
            {
                message = "Weekly leaderboard endpoint created"
            });
        }

        [HttpGet("my-rank/{userId}")]
        public IActionResult GetUserRank(string userId)
        {
            // TODO: implement user rank logic

            return Ok(new
            {
                userId = userId,
                rank = 0,
                message = "User rank endpoint created"
            });
        }
    }
}
