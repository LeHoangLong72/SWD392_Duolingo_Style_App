namespace NihongoLearning.Services
{
    public interface ILearningService
    {
        Task<IEnumerable<object>> GetJapanesePathAsync(int userId);
        Task UpdateProgressAsync(int userId, int lessonId);
    }
}
