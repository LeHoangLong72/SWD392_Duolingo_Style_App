using MyWebApiApp.Data;
using MyWebApiApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyWebApiApp.DTOs.Streak;

namespace MyWebApiApp.Repository
{
    public class StreakRepository : IStreakRepository
    {
        private readonly ApplicationDbContext _context;

        public StreakRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StreakResponse?> GetStreakAsync(string userId)
        {
            var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            return new StreakResponse
            {
                CurrentStreak = user.CurrentStreak,
                LongestStreak = user.LongestStreak,
                LastStudyDate = user.LastStudyDate
            };
        }

        public async Task UpdateStreakAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return;

            var today = DateTime.UtcNow.Date;

            if (user.LastStudyDate == null)
            {
                user.CurrentStreak = 1;
            }
            else
            {
                var diff = (today - user.LastStudyDate.Value.Date).Days;

                if (diff == 1)
                {
                    user.CurrentStreak++;
                }
                else if (diff > 1)
                {
                    user.CurrentStreak = 1;
                }
            }

            user.LastStudyDate = today;

            if (user.CurrentStreak > user.LongestStreak)
            {
                user.LongestStreak = user.CurrentStreak;
            }

            await _context.SaveChangesAsync();
        }
    }
}

