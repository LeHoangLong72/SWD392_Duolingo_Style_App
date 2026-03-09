namespace MyWebApiApp.Interfaces
{
    public interface IStreakRepository
    {
        Task UpdateStreakAsync(string userId);
    }
}
