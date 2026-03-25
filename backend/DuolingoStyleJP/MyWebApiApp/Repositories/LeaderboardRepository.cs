using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.DTOs.Leaderboard;
using MyWebApiApp.Interfaces;

namespace MyWebApiApp.Repository
{
    public class LeaderboardRepository : ILeaderboardRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaderboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeaderboardDto>> GetLeaderboardAsync()
        {
            var startOfWeek = DateTime.UtcNow.Date
        .AddDays(-(int)DateTime.UtcNow.DayOfWeek);

            var leaderboard = await _context.UserProgress
                .Where(p => p.CompletedDate >= startOfWeek)
                .GroupBy(p => new { p.UserId, p.User.UserName, p.User.Level })
                .Select(g => new LeaderboardDto
                {
                    UserId = g.Key.UserId,
                    Username = g.Key.UserName,
                    Level = g.Key.Level,
                    TotalXP = g.Sum(x => x.EarnedXP)
                })
                .OrderByDescending(x => x.TotalXP)
                .Take(10)
                .ToListAsync();

            for (int i = 0; i < leaderboard.Count; i++)
            {
                leaderboard[i].Rank = i + 1;
            }

            return leaderboard;
        }

        public async Task<List<LeaderboardDto>> GetWeeklyTop3Async()
        {
            var startOfWeek = DateTime.UtcNow.Date
                .AddDays(-(int)DateTime.UtcNow.DayOfWeek);
            var leaderboard = await _context.UserProgress
                .Where(p => p.CompletedDate >= startOfWeek)
                .GroupBy(p => new { p.UserId, p.User.UserName, p.User.Level })
                .Select(g => new LeaderboardDto
                {
                    UserId = g.Key.UserId,
                    Username = g.Key.UserName,
                    Level = g.Key.Level,
                    TotalXP = g.Sum(x => x.EarnedXP)
                })
                .OrderByDescending(x => x.TotalXP)
                .Take(3)
                .ToListAsync();

            for (int i = 0; i < leaderboard.Count; i++)
            {
                leaderboard[i].Rank = i + 1;
            }
            return leaderboard;
        }

        public async Task RewardWeeklyTopAsync()
        {
            var startOfWeek = DateTime.UtcNow.Date
                .AddDays(-(int)DateTime.UtcNow.DayOfWeek);

            var topUsers = await _context.UserProgress
                .Where(p => p.CompletedDate >= startOfWeek)
                .GroupBy(p => p.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    WeeklyXP = g.Sum(x => x.EarnedXP)
                })
                .OrderByDescending(x => x.WeeklyXP)
                .Take(10)
                .ToListAsync();

            int rank = 1;

            foreach (var player in topUsers)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == player.UserId);

                if (user == null) continue;

                int reward = 0;

                if (rank == 1)
                    reward = 100;
                else if (rank == 2)
                    reward = 70;
                else if (rank == 3)
                    reward = 50;
                else
                    reward = 20;

                user.Gems += reward;

                rank++;
            }

            await _context.SaveChangesAsync();
        }
    }
}
