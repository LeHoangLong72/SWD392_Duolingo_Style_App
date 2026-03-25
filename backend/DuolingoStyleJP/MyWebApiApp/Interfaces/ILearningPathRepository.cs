using MyWebApiApp.Models;

namespace MyWebApiApp.Interfaces
{
    public interface ILearningPathRepository
    {
        Task<List<Lesson>> GetAllLessonsAsync();
        Task<List<int>> GetCompletedLessonIdsAsync(string userId);
        Task<List<UserProgress>> GetUserProgressAsync(string userId);
        Task<List<UserMistake>> GetUserMistakesAsync(string userId);
    }
}
