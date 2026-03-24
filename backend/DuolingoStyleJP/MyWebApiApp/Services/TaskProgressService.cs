using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.Interfaces;

namespace MyWebApiApp.Services
{
    public class TaskProgressService : ITaskProgressService
    {
        private readonly ApplicationDbContext _context;

        public TaskProgressService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task HandleEventAsync(string userId, string eventType, int value)
        {
            var today = DateTime.UtcNow.Date;

            var userTasks = await _context.UserTasks
                .Include(x => x.Task)
                .Where(x =>
                    x.UserId == userId &&
                    x.AssignedDate == today &&
                    x.Task.TaskType == eventType &&
                    !x.IsCompleted)
                .ToListAsync();

            if (!userTasks.Any())
            {
                return;
            }

            foreach (var userTask in userTasks)
            {
                userTask.Progress += value;

                if (userTask.Progress >= userTask.Task.TargetValue)
                {
                    userTask.IsCompleted = true;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
