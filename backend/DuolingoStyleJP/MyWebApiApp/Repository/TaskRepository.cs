using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.Interfaces;
using MyWebApiApp.Models;

namespace MyWebApiApp.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> ClaimTaskRewardAsync(string userId, int taskId)
        {
            var today = DateTime.UtcNow.Date;

            var userTask = await _context.UserTasks
                .Include(x => x.Task)
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.TaskId == taskId &&
                    x.AssignedDate == today);

            if (userTask == null)
                throw new Exception("Task not found");

            if (!userTask.IsCompleted)
                throw new Exception("Task not completed");

            if (userTask.IsClaimed)
                throw new Exception("Reward already claimed");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            user.TotalXP += userTask.Task.RewardXP;
            user.Gems += userTask.Task.RewardGems;

            userTask.IsClaimed = true;

            await _context.SaveChangesAsync();

            return new
            {
                rewardXP = userTask.Task.RewardXP,
                rewardGems = userTask.Task.RewardGems
            };
        }

        public async Task<List<UserTask>> GetDailyTasksAsync(string userId)
        {
            var today = DateTime.UtcNow.Date;

            // Kiểm tra user đã có task hôm nay chưa
            var existingTasks = await _context.UserTasks
                .Include(x => x.Task)
                .Where(x => x.UserId == userId && x.AssignedDate == today)
                .ToListAsync();

            if (existingTasks.Any())
                return existingTasks;

            // Random 3 task
            var randomTasks = await _context.Tasks
                .Where(x => x.IsDaily)
                .OrderBy(x => Guid.NewGuid())
                .Take(3)
                .ToListAsync();

            var userTasks = new List<UserTask>();

            foreach (var task in randomTasks)
            {
                userTasks.Add(new UserTask
                {
                    UserId = userId,
                    TaskId = task.TaskId,
                    AssignedDate = today,
                    Progress = 0,
                    IsCompleted = false,
                    IsClaimed = false
                });
            }

            _context.UserTasks.AddRange(userTasks);
            await _context.SaveChangesAsync();

            return await _context.UserTasks
                .Include(x => x.Task)
                .Where(x => x.UserId == userId && x.AssignedDate == today)
                .ToListAsync();
        }

        public async Task<List<UserTask>> GetTaskProgressAsync(string userId)
        {
            return await _context.UserTasks
            .Include(x => x.Task)
            .Where(x => x.UserId == userId)
            .ToListAsync();
        }
    }
}
