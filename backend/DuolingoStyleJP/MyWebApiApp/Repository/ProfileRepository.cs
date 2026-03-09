using MyWebApiApp.Data;
using MyWebApiApp.DTOs.Profile;
using MyWebApiApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MyWebApiApp.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public ProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileResponse?> GetProfileAsync(string userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            return new ProfileResponse
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Level = user.Level,
                CurrentXP = user.CurrentXP,
                TotalXP = user.TotalXP,
                Gems = user.Gems,
                Hearts = user.Hearts,
                MaxHearts = user.MaxHearts,
                CurrentStreak = user.CurrentStreak,
                LongestStreak = user.LongestStreak
            };
        }

        public async Task<ProfileResponse?> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            if (!string.IsNullOrEmpty(request.UserName))
            {
                user.UserName = request.UserName;
            }

            await _context.SaveChangesAsync();

            return new ProfileResponse
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Level = user.Level,
                CurrentXP = user.CurrentXP,
                TotalXP = user.TotalXP,
                Gems = user.Gems,
                Hearts = user.Hearts,
                MaxHearts = user.MaxHearts,
                CurrentStreak = user.CurrentStreak,
                LongestStreak = user.LongestStreak
            };
        }
    }
}
