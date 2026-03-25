using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.Interfaces;
using MyWebApiApp.Models;
using System;

namespace MyWebApiApp.Repository
{
    public class LearningPathRepository : ILearningPathRepository
    {
        private readonly ApplicationDbContext _context;

        public LearningPathRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Lesson>> GetAllLessonsAsync()
        {
            return await _context.Lessons
                .OrderBy(l => l.LessonId)
                .ToListAsync();
        }

        public async Task<List<int>> GetCompletedLessonIdsAsync(string userId)
        {
            return await _context.UserProgress
                .Where(p => p.UserId == userId)
                .Select(p => p.LessonId)
                .ToListAsync();
        }

        public async Task<List<UserMistake>> GetUserMistakesAsync(string userId)
        {
            return await _context.UserMistakes
                .Where(x => x.UserId == userId)
                .Include(x => x.Question)
                    .ThenInclude(q => q.QuestionOptions)
                .ToListAsync();
        }

        public async Task<List<UserProgress>> GetUserProgressAsync(string userId)
        {
            return await _context.UserProgress
                .Where(p => p.UserId == userId)
                .Include(p => p.Lesson)
                .ToListAsync();
        }
    }
}
