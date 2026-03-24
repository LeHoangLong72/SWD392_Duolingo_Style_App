using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.DTOs.LessonContent;
using MyWebApiApp.Enums;
using MyWebApiApp.Interfaces;
using MyWebApiApp.Models;
using MyWebApiApp.Services;

namespace MyWebApiApp.Repository
{
    public class LessonAttemptRepository : ILessonAttemptRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IStreakRepository _streakRepo;
        private readonly IHeartRepository _heartRepo;
        private readonly IAchievementService _achievementService;
        private readonly ITaskProgressService _taskProgress;

        public LessonAttemptRepository(
            ApplicationDbContext context,
            IStreakRepository streakRepo,
            IHeartRepository heartRepo,
            IAchievementService achievementService,
            ITaskProgressService taskProgress)
        {
            _context = context;
            _streakRepo = streakRepo;
            _heartRepo = heartRepo;
            _achievementService = achievementService;
            _taskProgress = taskProgress;
        }

        public async Task<CompleteLessonResponse?> CompleteLessonAsync(string userId, int attemptId)
        {
            var attempt = await _context.LessonAttempts
                .FirstOrDefaultAsync(a =>
                    a.LessonAttemptId == attemptId &&
                    a.UserId == userId);

            if (attempt == null)
                return null;

            if (attempt.CompletedAt != null)
                throw new Exception("Bài học đã hoàn thành");

            attempt.CompletedAt = DateTime.UtcNow;

            double scorePercent = attempt.TotalQuestions == 0
                ? 0
                : (double)attempt.CorrectAnswers / attempt.TotalQuestions;

            attempt.IsPassed = scorePercent >= 0.8;

            int earnedXP = 0;
            bool isFirstLessonToday = false;

            if (attempt.IsPassed)
            {
                var lesson = await _context.Lessons
                    .FirstOrDefaultAsync(l => l.LessonId == attempt.LessonId);

                int baseXP = lesson?.BaseXP ?? 10;

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                    throw new Exception("Không tìm thấy user");

                isFirstLessonToday =
                    user.LastStudyDate == null ||
                    user.LastStudyDate.Value.Date < DateTime.UtcNow.Date;

                var existingProgress = await _context.UserProgress
                    .FirstOrDefaultAsync(p =>
                        p.UserId == userId &&
                        p.LessonId == attempt.LessonId);

                bool isFirstTimeLesson = existingProgress == null;

                if (isFirstTimeLesson)
                {
                    earnedXP = baseXP;

                    _context.UserProgress.Add(new UserProgress
                    {
                        UserId = userId,
                        LessonId = attempt.LessonId,
                        IsCompleted = true,
                        CompletedDate = DateTime.UtcNow,
                        EarnedXP = earnedXP
                    });

                    await _taskProgress.HandleEventAsync(
                        userId,
                        TaskEventTypes.NEW_LESSON,
                        1);
                }
                else
                {
                    earnedXP = (int)(baseXP * 0.3);

                    existingProgress.IsCompleted = true;
                    existingProgress.CompletedDate = DateTime.UtcNow;
                    existingProgress.EarnedXP += earnedXP;
                }

                // PERFECT LESSON
                if (attempt.CorrectAnswers == attempt.TotalQuestions)
                {
                    earnedXP += 5;

                    await _taskProgress.HandleEventAsync(
                        userId,
                        TaskEventTypes.PERFECT_LESSON,
                        1);
                }

                // ACCURACY
                double accuracy = attempt.TotalQuestions == 0
                    ? 0
                    : (double)attempt.CorrectAnswers / attempt.TotalQuestions;

                if (accuracy >= 0.9)
                {
                    await _taskProgress.HandleEventAsync(
                        userId,
                        TaskEventTypes.ACCURACY,
                        1);
                }

                // FAST LESSON
                var duration = (attempt.CompletedAt.Value - attempt.StartedAt).TotalSeconds;

                if (duration <= 30)
                {
                    await _taskProgress.HandleEventAsync(
                        userId,
                        TaskEventTypes.FAST_LESSON,
                        1);
                }

                // LESSON COMPLETE
                await _taskProgress.HandleEventAsync(
                    userId,
                    TaskEventTypes.LESSON_COMPLETE,
                    1);

                // XP BOOST
                var boost = await _context.UserItems
                    .Include(x => x.Item)
                    .FirstOrDefaultAsync(x =>
                        x.UserId == userId &&
                        x.Item.Name == "Double XP" &&
                        x.ExpiredAt > DateTime.UtcNow);

                if (boost != null)
                {
                    earnedXP *= 2;
                }

                user.CurrentXP += earnedXP;
                user.TotalXP += earnedXP;

                await _taskProgress.HandleEventAsync(
                    userId,
                    TaskEventTypes.EARN_XP,
                    earnedXP);

                int xpNeeded = user.Level * 100;

                while (user.CurrentXP >= xpNeeded)
                {
                    user.CurrentXP -= xpNeeded;
                    user.Level++;
                    xpNeeded = user.Level * 100;
                }

                await _streakRepo.UpdateStreakAsync(userId);
            }

            await _context.SaveChangesAsync();

            await _achievementService.CheckLessonAchievementsAsync(userId);

            return new CompleteLessonResponse
            {
                TotalQuestions = attempt.TotalQuestions,
                CorrectAnswers = attempt.CorrectAnswers,
                IsPassed = attempt.IsPassed,
                EarnedXP = earnedXP,
                IsStreakIncreased = isFirstLessonToday
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
                .FirstOrDefaultAsync(a =>
                    a.LessonAttemptId == attemptId &&
                    a.UserId == userId);

            if (attempt == null)
                throw new Exception("Invalid attempt");

            var option = await _context.QuestionOptions
                .FirstOrDefaultAsync(o =>
                    o.OptionId == request.SelectedOptionId &&
                    o.QuestionId == request.QuestionId);

            if (option == null)
                throw new Exception("Lựa chọn không hợp lệ");

            var userAnswer = new UserAnswer
            {
                LessonAttemptId = attemptId,
                QuestionId = request.QuestionId,
                SelectedOptionId = request.SelectedOptionId,
                IsCorrect = option.IsCorrect
            };

            _context.UserAnswers.Add(userAnswer);

            if (option.IsCorrect)
            {
                attempt.CorrectAnswers++;

                await _taskProgress.HandleEventAsync(
                    userId,
                    TaskEventTypes.CORRECT_ANSWER,
                    1);
            }
            else
            {
                var mistake = await _context.UserMistakes
                    .FirstOrDefaultAsync(x =>
                        x.UserId == userId &&
                        x.QuestionId == request.QuestionId);

                if (mistake == null)
                {
                    _context.UserMistakes.Add(new UserMistake
                    {
                        UserId = userId,
                        QuestionId = request.QuestionId,
                        WrongCount = 1,
                        LastWrongAt = DateTime.UtcNow
                    });
                }
                else
                {
                    mistake.WrongCount++;
                    mistake.LastWrongAt = DateTime.UtcNow;
                }

                var heartRemaining = await _heartRepo.LoseHeartAsync(userId);

                if (!heartRemaining)
                    throw new Exception("Hết tim rồi!");
            }

            await _context.SaveChangesAsync();

            return new SubmitAnswerResponse
            {
                IsCorrect = option.IsCorrect
            };
        }
    }
}
