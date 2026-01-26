using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NihongoLearning.DTOs;
using NihongoLearning.Models;

namespace NihongoLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AchievementsController> _logger;

        public AchievementsController(AppDbContext context, ILogger<AchievementsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Lấy tất cả achievements của user (kể cả chưa unlock)
        /// GET: api/achievements?userId=1
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AchievementDto>>> GetUserAchievements([FromQuery] int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "Không tìm thấy user" });

            var achievements = await _context.Achievements
                .Where(a => a.IsActive)
                .OrderBy(a => a.OrderIndex)
                .Select(a => new
                {
                    a.AchievementId,
                    a.AchievementName,
                    a.Description,
                    a.Category,
                    a.TargetValue,
                    a.IconUrl,
                    a.RewardGems,
                    a.RewardXp,
                    a.Rarity
                })
                .ToListAsync();

            var userAchievements = await _context.UserAchievements
                .Where(ua => ua.UserId == userId)
                .ToDictionaryAsync(ua => ua.AchievementId, ua => ua);

            var result = achievements.Select(a =>
            {
                var userAchievement = userAchievements.GetValueOrDefault(a.AchievementId);
                var currentProgress = userAchievement?.CurrentProgress ?? 0;

                return new AchievementDto
                {
                    AchievementId = a.AchievementId,
                    AchievementName = a.AchievementName,
                    Description = a.Description,
                    Category = a.Category,
                    TargetValue = a.TargetValue,
                    IconUrl = a.IconUrl,
                    RewardGems = a.RewardGems,
                    RewardXp = a.RewardXp,
                    Rarity = a.Rarity,
                    CurrentProgress = currentProgress,
                    IsUnlocked = userAchievement?.IsUnlocked ?? false,
                    UnlockedDate = userAchievement?.UnlockedDate,
                    ProgressPercentage = Math.Min(100, (int)((double)currentProgress / a.TargetValue * 100))
                };
            }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách achievements đã unlock
        /// GET: api/achievements/unlocked?userId=1
        /// </summary>
        [HttpGet("unlocked")]
        public async Task<ActionResult<IEnumerable<AchievementDto>>> GetUnlockedAchievements([FromQuery] int userId)
        {
            var unlockedAchievements = await _context.UserAchievements
                .Include(ua => ua.Achievement)
                .Where(ua => ua.UserId == userId && ua.IsUnlocked)
                .OrderByDescending(ua => ua.UnlockedDate)
                .Select(ua => new AchievementDto
                {
                    AchievementId = ua.AchievementId,
                    AchievementName = ua.Achievement.AchievementName,
                    Description = ua.Achievement.Description,
                    Category = ua.Achievement.Category,
                    TargetValue = ua.Achievement.TargetValue,
                    IconUrl = ua.Achievement.IconUrl,
                    RewardGems = ua.Achievement.RewardGems,
                    RewardXp = ua.Achievement.RewardXp,
                    Rarity = ua.Achievement.Rarity,
                    CurrentProgress = ua.CurrentProgress,
                    IsUnlocked = true,
                    UnlockedDate = ua.UnlockedDate,
                    ProgressPercentage = 100
                })
                .ToListAsync();

            return Ok(unlockedAchievements);
        }

        /// <summary>
        /// Lấy thống kê achievements
        /// GET: api/achievements/stats?userId=1
        /// </summary>
        [HttpGet("stats")]
        public async Task<ActionResult> GetAchievementStats([FromQuery] int userId)
        {
            var totalAchievements = await _context.Achievements.CountAsync(a => a.IsActive);

            var unlockedCount = await _context.UserAchievements
                .CountAsync(ua => ua.UserId == userId && ua.IsUnlocked);

            var stats = new
            {
                TotalAchievements = totalAchievements,
                UnlockedCount = unlockedCount,
                LockedCount = totalAchievements - unlockedCount,
                CompletionPercentage = totalAchievements > 0
                    ? (int)((double)unlockedCount / totalAchievements * 100)
                    : 0
            };

            return Ok(stats);
        }

        /// <summary>
        /// Check và unlock achievement (được gọi tự động sau các hành động)
        /// POST: api/achievements/check
        /// </summary>
        [HttpPost("check")]
        public async Task<ActionResult<List<UnlockAchievementResponse>>> CheckAndUnlockAchievements([FromBody] CheckAchievementRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return NotFound(new { message = "Không tìm thấy user" });

            var unlockedAchievements = new List<UnlockAchievementResponse>();

            // Lấy tất cả achievements chưa unlock
            var pendingAchievements = await _context.Achievements
                .Where(a => a.IsActive)
                .Where(a => !_context.UserAchievements.Any(ua =>
                    ua.UserId == request.UserId &&
                    ua.AchievementId == a.AchievementId &&
                    ua.IsUnlocked))
                .ToListAsync();

            foreach (var achievement in pendingAchievements)
            {
                var currentProgress = await CalculateProgress(request.UserId, achievement);

                // Tìm hoặc tạo mới UserAchievement
                var userAchievement = await _context.UserAchievements
                    .FirstOrDefaultAsync(ua => ua.UserId == request.UserId && ua.AchievementId == achievement.AchievementId);

                if (userAchievement == null)
                {
                    userAchievement = new UserAchievement
                    {
                        UserId = request.UserId,
                        AchievementId = achievement.AchievementId,
                        CurrentProgress = currentProgress
                    };
                    _context.UserAchievements.Add(userAchievement);
                }
                else
                {
                    userAchievement.CurrentProgress = currentProgress;
                }

                // Check unlock
                if (currentProgress >= achievement.TargetValue && !userAchievement.IsUnlocked)
                {
                    userAchievement.IsUnlocked = true;
                    userAchievement.UnlockedDate = DateTime.Now;

                    // Cộng rewards
                    user.Gems = (user.Gems ?? 0) + achievement.RewardGems;
                    user.TotalXp = (user.TotalXp ?? 0) + achievement.RewardXp;

                    unlockedAchievements.Add(new UnlockAchievementResponse
                    {
                        AchievementId = achievement.AchievementId,
                        AchievementName = achievement.AchievementName,
                        Description = achievement.Description,
                        Rarity = achievement.Rarity,
                        RewardGems = achievement.RewardGems,
                        RewardXp = achievement.RewardXp,
                        TotalGems = user.Gems ?? 0,
                        TotalXp = user.TotalXp ?? 0
                    });

                    _logger.LogInformation("User {UserId} đạt achievement: {AchievementName}",
                        request.UserId, achievement.AchievementName);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(unlockedAchievements);
        }

        /// <summary>
        /// Tính progress dựa trên condition của achievement
        /// </summary>
        private async Task<int> CalculateProgress(int userId, Achievement achievement)
        {
            var parts = achievement.Condition.Split(':');
            if (parts.Length != 2) return 0;

            var conditionType = parts[0];

            return conditionType switch
            {
                "CompleteLesson" => await _context.UserLessonProgresses
                    .CountAsync(ulp => ulp.UserId == userId && ulp.IsCompleted == true),

                "LearnKanji" => await _context.UserKanjiProgresses
                    .CountAsync(ukp => ukp.UserId == userId && ukp.IsLearned == true),

                "TotalXp" => await _context.Users
                    .Where(u => u.UserId == userId)
                    .Select(u => u.TotalXp ?? 0)
                    .FirstOrDefaultAsync(),

                "Streak" => await _context.Users
                    .Where(u => u.UserId == userId)
                    .Select(u => u.StreakCount ?? 0)
                    .FirstOrDefaultAsync(),

                "CollectGems" => await _context.Users
                    .Where(u => u.UserId == userId)
                    .Select(u => u.Gems ?? 0)
                    .FirstOrDefaultAsync(),

                "PerfectScore" => await _context.UserLessonProgresses
                    .CountAsync(ulp => ulp.UserId == userId && ulp.Score == 100),

                "ThreeStars" => await _context.UserLessonProgresses
                    .CountAsync(ulp => ulp.UserId == userId && ulp.Stars == 3),

                "CompleteTopic" => await _context.UserLessonProgresses
                    .Where(ulp => ulp.UserId == userId && ulp.IsCompleted == true)
                    .Select(ulp => ulp.Lesson.TopicId)
                    .Distinct()
                    .CountAsync(),

                _ => 0
            };
        }
    }
}
