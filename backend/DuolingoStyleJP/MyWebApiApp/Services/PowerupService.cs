using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.Interfaces;

namespace MyWebApiApp.Services
{
    public class PowerupService : IPowerupService
    {
        private readonly ApplicationDbContext _context;
        

        public PowerupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ActivatePowerupAsync(string userId, int userItemId)
        {
            var now = DateTime.UtcNow;

            var powerup = await _context.UserItems
                .Include(ui => ui.Item)
                .FirstOrDefaultAsync(ui => ui.Id == userItemId && ui.UserId == userId);

            if (powerup == null || powerup.Item.Category.ToLower() != "powerup")
                return;

            // Gán thời gian hết hạn (đã có DurationMinutes trong Item)
            powerup.ActivatedAt = now;
            powerup.ExpiredAt = now.AddMinutes(powerup.Item.DurationMinutes ?? 0);

            // Mark đã dùng (IsConsumed) nếu muốn xóa khỏi Inventory
            powerup.IsConsumed = true;

            // Save ngay để Equip xong là có tác dụng
            await _context.SaveChangesAsync();
        }

        public async Task<int> ApplyPowerupsAsync(string userId, int baseXP)
        {
            var now = DateTime.UtcNow;

            // Lấy powerup đang active
            var activePowerups = await _context.UserItems
                .Include(ui => ui.Item)
                .Where(ui =>
                    ui.UserId == userId &&
                    ui.Item.Category == "powerup" &&
                    ui.ActivatedAt != null &&
                    ui.ExpiredAt != null &&
                    ui.ActivatedAt <= now &&
                    ui.ExpiredAt >= now &&
                    !ui.IsConsumed) // optional: nếu bạn vẫn muốn hiện trong inventory khi active
                .ToListAsync();

            int finalXP = baseXP;

            foreach (var powerup in activePowerups)
            {
                switch (powerup.Item.Name.ToLower())
                {
                    case "double xp":
                        finalXP *= 2;
                        powerup.IsConsumed = true;
                        break;

                    case "freeze streak":
                        //await FreezeUserStreakAsync(userId, powerup.ExpiredAt.Value);
                        break;

                        // Thêm các loại powerup khác nếu cần
                }
            }

            if (activePowerups.Any())
            {
                await _context.SaveChangesAsync();
            }

            return finalXP;
        }

        
    }
}
