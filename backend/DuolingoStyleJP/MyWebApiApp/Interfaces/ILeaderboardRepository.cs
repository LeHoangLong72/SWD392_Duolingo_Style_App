using MyWebApiApp.DTOs.Leaderboard;

namespace MyWebApiApp.Interfaces
{
    public interface ILeaderboardRepository
    {
        Task<List<LeaderboardDto>> GetLeaderboardAsync();
        Task<List<LeaderboardDto>> GetWeeklyTop3Async();
        Task RewardWeeklyTopAsync();
    }
}
