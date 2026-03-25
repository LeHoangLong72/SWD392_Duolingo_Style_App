using MyWebApiApp.Models;

namespace MyWebApiApp.Interfaces
{
    public interface IAchievementService
    {
        Task<List<Achievement>> CheckLessonAchievementsAsync(string userId);
        Task<List<Achievement>> CheckTotalXPAchievementsAsync(string userId);
    }
}