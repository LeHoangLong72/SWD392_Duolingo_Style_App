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

        public async Task CheckLessonAchievementsAsync(string userId)
        {
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
                    _context.UserAchievements.Add(new UserAchievement
                    {
                        UserId = userId,
                        AchievementId = achievement.AchievementId,
                        UnlockedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
