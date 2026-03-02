using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.DTOs.LessonContent;
using MyWebApiApp.Interfaces;
using MyWebApiApp.Models;

namespace MyWebApiApp.Repository
{
    public class LessonContentRepository : ILessonContentRepository
    {
        private readonly ApplicationDbContext _context;
        public LessonContentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CompleteLessonResponse?> CompleteLessonAsync(string userId, int attemptId)
        {
            var attempt = await _context.LessonAttempts
        .FirstOrDefaultAsync(a =>
            a.LessonAttemptId == attemptId
            && a.UserId == userId);

            if (attempt == null)
                return null;

            // Nếu đã complete rồi thì không cho complete lại
            if (attempt.CompletedAt != null)
                throw new Exception("Lesson already completed.");

            attempt.CompletedAt = DateTime.UtcNow;

            double scorePercent = attempt.TotalQuestions == 0
                ? 0
                : (double)attempt.CorrectAnswers / attempt.TotalQuestions;

            attempt.IsPassed = scorePercent >= 0.8;

            int earnedXP = 0;

            if (attempt.IsPassed)
            {
                // Lấy XP gốc của lesson
                var lesson = await _context.Lessons
                    .FirstOrDefaultAsync(l => l.LessonId == attempt.LessonId);

                earnedXP = lesson?.BaseXP ?? 10; // fallback 10 XP nếu null

                // Kiểm tra đã có progress chưa
                var existingProgress = await _context.UserProgress
                    .FirstOrDefaultAsync(p =>
                        p.UserId == userId &&
                        p.LessonId == attempt.LessonId);

                if (existingProgress == null)
                {
                    _context.UserProgress.Add(new UserProgress
                    {
                        UserId = userId,
                        LessonId = attempt.LessonId,
                        Status = true,
                        CompletedDate = DateTime.UtcNow,
                        EarnedXP = earnedXP
                    });
                }
                else
                {
                    // Nếu đã có thì chỉ update (trường hợp học lại)
                    existingProgress.Status = true;
                    existingProgress.CompletedDate = DateTime.UtcNow;
                    existingProgress.EarnedXP = earnedXP;
                }
            }

            await _context.SaveChangesAsync();

            return new CompleteLessonResponse
            {
                TotalQuestions = attempt.TotalQuestions,
                CorrectAnswers = attempt.CorrectAnswers,
                IsPassed = attempt.IsPassed
            };
        }

        public async Task<LessonContentDto?> GetLessonContentAsync(int lessonId)
        {
            var lesson = await _context.Lessons
            .Include(l => l.Questions)
                .ThenInclude(q => q.QuestionOptions)
            .FirstOrDefaultAsync(l => l.LessonId == lessonId);

            if (lesson == null) return null;

            return new LessonContentDto
            {
                AttemptId = 0,
                Questions = lesson.Questions
                    .OrderBy(q => q.OrderIndex)
                    .Select(q => new QuestionDto
                    {
                        QuestionId = q.QuestionId,
                        Content = q.Content,
                        Options = q.QuestionOptions
                            .Select(o => new OptionDto
                            {
                                OptionId = o.OptionId,
                                OptionText = o.OptionText
                            }).ToList()
                    }).ToList()
            };
        }

        public async Task<LessonContentDto?> StartLessonAsync(string userId, int lessonId)
        {
            var lesson = await _context.Lessons
            .Include(l => l.Questions)
                .ThenInclude(q => q.QuestionOptions)
            .FirstOrDefaultAsync(l => l.LessonId == lessonId);

            if (lesson == null) return null;

            var attempt = new LessonAttempt
            {
                UserId = userId,
                LessonId = lessonId,
                StartedAt = DateTime.UtcNow,
                TotalQuestions = lesson.Questions.Count,
                CorrectAnswers = 0,
                IsPassed = false
            };

            _context.LessonAttempts.Add(attempt);
            await _context.SaveChangesAsync();

            return new LessonContentDto
            {
                AttemptId = attempt.LessonAttemptId,
                Questions = lesson.Questions
                    .OrderBy(q => q.OrderIndex)
                    .Select(q => new QuestionDto
                    {
                        QuestionId = q.QuestionId,
                        Content = q.Content,
                        Options = q.QuestionOptions
                            .Select(o => new OptionDto
                            {
                                OptionId = o.OptionId,
                                OptionText = o.OptionText
                            }).ToList()
                    }).ToList()
            };
        }

        public async Task<SubmitAnswerResponse> SubmitAnswerAsync(string userId, int attemptId, SubmitAnswerRequest request)
        {
            var attempt = await _context.LessonAttempts
            .FirstOrDefaultAsync(a => a.LessonAttemptId == attemptId && a.UserId == userId);

            if (attempt == null)
                throw new Exception("Invalid attempt");

            var option = await _context.QuestionOptions
                .FirstOrDefaultAsync(o =>
                    o.OptionId == request.SelectedOptionId &&
                    o.QuestionId == request.QuestionId);

            if (option == null)
                throw new Exception("Invalid option");

            var userAnswer = new UserAnswer
            {
                LessonAttemptId = attemptId,
                QuestionId = request.QuestionId,
                SelectedOptionId = request.SelectedOptionId,
                IsCorrect = option.IsCorrect
            };

            if (option.IsCorrect)
                attempt.CorrectAnswers++;

            _context.UserAnswers.Add(userAnswer);
            await _context.SaveChangesAsync();

            return new SubmitAnswerResponse
            {
                IsCorrect = option.IsCorrect
            };
        }
    }
}
