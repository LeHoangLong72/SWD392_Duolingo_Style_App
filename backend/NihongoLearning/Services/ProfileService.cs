using Microsoft.EntityFrameworkCore;
using NihongoLearning.Data;
using NihongoLearning.DTOs;

namespace NihongoLearning.Services
{
    public class ProfileService
    {
        private readonly AppDbContext _db;

        public ProfileService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<UserProfileDto> GetProfileByUserIdAsync(int userId)
        {
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserID == userId);
            if (profile == null) return null;

            return new UserProfileDto
            {
                UserID = profile.UserID,
                DisplayName = profile.DisplayName,
                AvatarUrl = profile.AvatarUrl,
                XP = profile.XP,
                Gems = profile.Gems,
                StreakCount = profile.StreakCount
            };
        }

        public async Task<bool> UpdateProfileAsync(int userId, UpdateProfileRequest req)
        {
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserID == userId);
            if (profile == null) return false;

            profile.DisplayName = req.DisplayName ?? profile.DisplayName;
            profile.AvatarUrl = req.AvatarUrl ?? profile.AvatarUrl;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserProfileDto>> GetAllProfilesAsync()
        {
            return await _db.UserProfiles
                .Select(p => new UserProfileDto
                {
                    UserID = p.UserID,
                    DisplayName = p.DisplayName,
                    AvatarUrl = p.AvatarUrl,
                    XP = p.XP,
                    Gems = p.Gems,
                    StreakCount = p.StreakCount
                })
                .ToListAsync();
        }
    }
}
