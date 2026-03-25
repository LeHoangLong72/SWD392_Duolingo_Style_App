using MyWebApiApp.Data;
using MyWebApiApp.Interfaces;
using MyWebApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MyWebApiApp.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly ApplicationDbContext _context;

        public AchievementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Achievement>> CheckLessonAchievementsAsync(string userId)
        {
            var newlyUnlocked = new List<Achievement>();
            // đếm số lesson user đã hoàn thành
            var lessonCompleted = await _context.UserProgress
                .CountAsync(x => x.UserId == userId && x.IsCompleted);

            // lấy achievement thuộc loại lesson
            var achievements = await _context.Achievements
                .Where(a => a.AchievementType == "LESSON_COMPLETE")
                .ToListAsync();

            foreach (var achievement in achievements)
            {
                bool alreadyUnlocked = await _context.UserAchievements
                    .AnyAsync(x => x.UserId == userId && x.AchievementId == achievement.AchievementId);

                if (alreadyUnlocked)
                    continue;

                if (lessonCompleted >= achievement.RequiredValue)
                {
                    var userAchievement = new UserAchievement
                    {
                        UserId = userId,
                        AchievementId = achievement.AchievementId,
                        UnlockedAt = DateTime.UtcNow
                    };

                    await _context.UserAchievements.AddAsync(userAchievement);
                    newlyUnlocked.Add(achievement);
                }
            }

            await _context.SaveChangesAsync();
            return newlyUnlocked;
        }

        public async Task<List<Achievement>> CheckTotalXPAchievementsAsync(string userId)
        {
            var newlyUnlocked = new List<Achievement>();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if(user == null)
            {
                return newlyUnlocked;
            }

            int totalXP = user.TotalXP;

            var xpAchievements = await _context.Achievements
                .Where(a => a.AchievementType == "TOTAL_XP")
                .ToListAsync();

            foreach (var achievement in xpAchievements)
            {
                bool alreadyUnlocked = await _context.UserAchievements
                    .AnyAsync(x => x.UserId == userId && x.AchievementId == achievement.AchievementId);

                if (alreadyUnlocked)
                {
                    continue;
                }

                if (totalXP >= achievement.RequiredValue)
                {
                    var userAchievement = new UserAchievement
                    {
                        UserId = userId,
                        AchievementId = achievement.AchievementId,
                        UnlockedAt = DateTime.UtcNow
                    };

                    await _context.UserAchievements.AddAsync(userAchievement);
                    newlyUnlocked.Add(achievement);
                }
            }

            await _context.SaveChangesAsync();
            return newlyUnlocked;
        }
    }
}
