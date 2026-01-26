using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NihongoLearning.DTOs;
using NihongoLearning.Models;

namespace NihongoLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LearningController> _logger;

        public LearningController(AppDbContext context, ILogger<LearningController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Bắt đầu học một bài - Tạo session và lấy danh sách câu hỏi
        /// GET: api/learning/start?userId=1&lessonId=1
        /// </summary>
        [HttpPost("start")]
        public async Task<ActionResult<StartLearningResponse>> StartLearning([FromBody] StartLearningRequest request)
        {
            try
            {
                // Kiểm tra user
                var user = await _context.Users.FindAsync(request.UserId);
                if (user == null)
                    return NotFound(new { message = "Không tìm thấy user" });

                // Kiểm tra lesson
                var lesson = await _context.Lessons
                    .Include(l => l.Topic)
                    .FirstOrDefaultAsync(l => l.LessonId == request.LessonId && l.IsActive == true);

                if (lesson == null)
                    return NotFound(new { message = "Không tìm thấy bài học" });

                // Kiểm tra bài trước đã hoàn thành chưa (unlock logic)
                var previousLesson = await _context.Lessons
                    .Where(l => l.TopicId == lesson.TopicId && l.OrderIndex < lesson.OrderIndex)
                    .OrderByDescending(l => l.OrderIndex)
                    .FirstOrDefaultAsync();

                if (previousLesson != null)
                {
                    var prevProgress = await _context.UserLessonProgresses
                        .FirstOrDefaultAsync(p => p.UserId == request.UserId
                            && p.LessonId == previousLesson.LessonId
                            && p.IsCompleted == true);

                    if (prevProgress == null)
                        return BadRequest(new { message = "Bạn cần hoàn thành bài học trước!" });
                }

                // Lấy danh sách câu hỏi
                var questions = await _context.Questions
                    .Include(q => q.QuestionOptions)
                    .Where(q => q.LessonId == request.LessonId)
                    .OrderBy(q => q.OrderIndex)
                    .ToListAsync();

                if (!questions.Any())
                    return NotFound(new { message = "Bài học chưa có câu hỏi. Vui lòng thử lại sau!" });

                // Xóa session cũ chưa hoàn thành (nếu có)
                var oldSessions = await _context.LearningSessions
                    .Where(s => s.UserId == request.UserId
                        && s.LessonId == request.LessonId
                        && s.Status == "InProgress")
                    .ToListAsync();

                if (oldSessions.Any())
                {
                    _context.LearningSessions.RemoveRange(oldSessions);
                }

                // Tạo learning session mới
                var session = new LearningSession
                {
                    UserId = request.UserId,
                    LessonId = request.LessonId,
                    StartTime = DateTime.Now,
                    TotalQuestions = questions.Count,
                    CorrectAnswers = 0,
                    Lives = 5,
                    Status = "InProgress"
                };

                _context.LearningSessions.Add(session);
                await _context.SaveChangesAsync();

                // Map sang DTO (không trả về đáp án đúng)
                var response = new StartLearningResponse
                {
                    SessionId = session.SessionId,
                    LessonName = lesson.LessonName,
                    TotalQuestions = questions.Count,
                    Lives = session.Lives,
                    Questions = questions.Select(q => new QuestionDto
                    {
                        QuestionId = q.QuestionId,
                        QuestionType = q.QuestionType,
                        QuestionText = q.QuestionText,
                        AudioUrl = q.AudioUrl,
                        ImageUrl = q.ImageUrl,
                        Options = q.QuestionOptions
                            .OrderBy(o => o.OrderIndex)
                            .Select(o => new OptionDto
                            {
                                OptionId = o.OptionId,
                                OptionText = o.OptionText
                            }).ToList()
                    }).ToList()
                };

                _logger.LogInformation("User {UserId} bắt đầu học bài {LessonId}, SessionId: {SessionId}",
                    request.UserId, request.LessonId, session.SessionId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi bắt đầu học bài {LessonId}", request.LessonId);
                return StatusCode(500, new { message = "Có lỗi xảy ra. Vui lòng thử lại!" });
            }
        }

        /// <summary>
        /// Submit câu trả lời
        /// POST: api/learning/submit-answer
        /// </summary>
        [HttpPost("submit-answer")]
        public async Task<ActionResult<SubmitAnswerResponse>> SubmitAnswer([FromBody] SubmitAnswerRequest request)
        {
            try
            {
                var session = await _context.LearningSessions
                    .FirstOrDefaultAsync(s => s.SessionId == request.SessionId);

                if (session == null)
                    return NotFound(new { message = "Phiên học không tồn tại" });

                if (session.Status != "InProgress")
                    return BadRequest(new { message = "Phiên học đã kết thúc" });

                // Lấy câu hỏi và đáp án
                var question = await _context.Questions
                    .Include(q => q.QuestionOptions)
                    .FirstOrDefaultAsync(q => q.QuestionId == request.QuestionId);

                if (question == null)
                    return NotFound(new { message = "Không tìm thấy câu hỏi" });

                var selectedOption = question.QuestionOptions
                    .FirstOrDefault(o => o.OptionId == request.SelectedOptionId);

                if (selectedOption == null)
                    return BadRequest(new { message = "Đáp án không hợp lệ" });

                var correctOption = question.QuestionOptions.First(o => o.IsCorrect);
                bool isCorrect = selectedOption.IsCorrect;

                // Cập nhật session
                if (isCorrect)
                {
                    session.CorrectAnswers++;
                }
                else
                {
                    session.Lives--;
                    if (session.Lives <= 0)
                    {
                        session.Status = "Failed";
                        session.EndTime = DateTime.Now;
                    }
                }

                await _context.SaveChangesAsync();

                int progressPercentage = (int)((double)session.CorrectAnswers / session.TotalQuestions * 100);

                var response = new SubmitAnswerResponse
                {
                    IsCorrect = isCorrect,
                    CorrectOptionId = correctOption.OptionId,
                    CorrectAnswer = correctOption.OptionText,
                    Explanation = isCorrect
                        ? "✅ Chính xác! Tuyệt vời!"
                        : $"❌ Sai rồi! Đáp án đúng là: {correctOption.OptionText}",
                    CurrentLives = session.Lives,
                    CorrectAnswers = session.CorrectAnswers,
                    TotalQuestions = session.TotalQuestions,
                    ProgressPercentage = progressPercentage,
                    SessionCompleted = false,
                    SessionFailed = session.Status == "Failed"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi submit câu trả lời");
                return StatusCode(500, new { message = "Có lỗi xảy ra. Vui lòng thử lại!" });
            }
        }

        /// <summary>
        /// Kết thúc phiên học và tính điểm
        /// POST: api/learning/complete/1
        /// </summary>
        [HttpPost("complete/{sessionId}")]
        public async Task<ActionResult<CompleteLearningSessionResponse>> CompleteSession(int sessionId)
        {
            try
            {
                var session = await _context.LearningSessions
                    .Include(s => s.Lesson)
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId);

                if (session == null)
                    return NotFound(new { message = "Không tìm thấy phiên học" });

                if (session.Status == "Completed")
                    return BadRequest(new { message = "Phiên học đã kết thúc trước đó" });

                // Tính điểm
                int score = (int)((double)session.CorrectAnswers / session.TotalQuestions * 100);

                int stars = score switch
                {
                    >= 90 => 3,
                    >= 70 => 2,
                    >= 50 => 1,
                    _ => 0
                };

                // Cập nhật session
                session.Status = score >= 50 ? "Completed" : "Failed";
                session.EndTime = DateTime.Now;

                // Cập nhật hoặc tạo mới UserLessonProgress
                var existingProgress = await _context.UserLessonProgresses
                    .FirstOrDefaultAsync(p => p.UserId == session.UserId && p.LessonId == session.LessonId);

                bool isNewRecord = false;

                if (existingProgress == null)
                {
                    existingProgress = new UserLessonProgress
                    {
                        UserId = session.UserId,
                        LessonId = session.LessonId,
                        IsCompleted = score >= 50,
                        Score = score,
                        Stars = stars,
                        CompletedDate = score >= 50 ? DateTime.Now : null,
                        CreatedDate = DateTime.Now
                    };
                    _context.UserLessonProgresses.Add(existingProgress);
                    isNewRecord = true;
                }
                else if (score > (existingProgress.Score ?? 0))
                {
                    existingProgress.Score = score;
                    existingProgress.Stars = stars;
                    existingProgress.IsCompleted = score >= 50;
                    existingProgress.CompletedDate = score >= 50 ? DateTime.Now : null;
                    isNewRecord = true;
                }

                // Cộng XP và Gems
                int xpEarned = 0;
                int gemsEarned = 0;

                if (isNewRecord && score >= 50)
                {
                    xpEarned = session.Lesson.XpReward ?? 10;
                    gemsEarned = session.Lesson.GemsReward ?? 5;

                    // Bonus theo điểm
                    if (score >= 90)
                    {
                        xpEarned += 5;
                        gemsEarned += 2;
                    }
                    else if (score >= 70)
                    {
                        xpEarned += 3;
                        gemsEarned += 1;
                    }

                    session.User.TotalXp = (session.User.TotalXp ?? 0) + xpEarned;
                    session.User.Gems = (session.User.Gems ?? 0) + gemsEarned;
                    session.User.LastLearnedDate = DateTime.Now;

                    // Cập nhật streak
                    var lastLearned = session.User.LastLearnedDate;
                    if (lastLearned.HasValue && lastLearned.Value.Date == DateTime.Today.AddDays(-1))
                    {
                        session.User.StreakCount = (session.User.StreakCount ?? 0) + 1;
                    }
                    else if (!lastLearned.HasValue || lastLearned.Value.Date != DateTime.Today)
                    {
                        session.User.StreakCount = 1;
                    }
                }

                await _context.SaveChangesAsync();

                try
                {
                    var achievementResponse = await CheckAchievementsForUser(session.UserId);
                    if (achievementResponse.Any())
                    {
                        _logger.LogInformation("User {UserId} đạt {Count} achievements mới",
                            session.UserId, achievementResponse.Count);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Lỗi khi check achievements cho user {UserId}", session.UserId);
                }

               

                var response = new CompleteLearningSessionResponse
                {
                    Score = score,
                    Stars = stars,
                    XpEarned = xpEarned,
                    GemsEarned = gemsEarned,
                    TotalXp = session.User.TotalXp ?? 0,
                    TotalGems = session.User.Gems ?? 0,
                    IsNewRecord = isNewRecord,
                    Message = stars switch
                    {
                        3 => "🌟 Xuất sắc! Hoàn hảo 100%!",
                        2 => "⭐ Tốt lắm! Tiếp tục phát huy!",
                        1 => "✨ Cố gắng thêm nhé!",
                        _ => "❌ Hãy thử lại để đạt điểm cao hơn!"
                    }
                };

                _logger.LogInformation("User {UserId} hoàn thành session {SessionId}: {Score}%, {Stars} sao",
                    session.UserId, sessionId, score, stars);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi hoàn thành phiên học {SessionId}", sessionId);
                return StatusCode(500, new { message = "Có lỗi xảy ra. Vui lòng thử lại!" });
            }
        }

        /// <summary>
        /// Hủy phiên học hiện tại
        /// DELETE: api/learning/cancel/1
        /// </summary>
        [HttpDelete("cancel/{sessionId}")]
        public async Task<ActionResult> CancelSession(int sessionId)
        {
            try
            {
                var session = await _context.LearningSessions.FindAsync(sessionId);
                if (session == null)
                    return NotFound(new { message = "Không tìm thấy phiên học" });

                if (session.Status != "InProgress")
                    return BadRequest(new { message = "Phiên học đã kết thúc" });

                _context.LearningSessions.Remove(session);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Session {SessionId} đã bị hủy", sessionId);

                return Ok(new { message = "Đã hủy phiên học" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi hủy phiên học {SessionId}", sessionId);
                return StatusCode(500, new { message = "Có lỗi xảy ra" });
            }
        }

        private async Task<List<UnlockAchievementResponse>> CheckAchievementsForUser(int userId)
        {
            // Gọi achievement check logic (tương tự AchievementsController)
            // Hoặc inject IAchievementService
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return new List<UnlockAchievementResponse>();

            var unlockedAchievements = new List<UnlockAchievementResponse>();

            var pendingAchievements = await _context.Achievements
                .Where(a => a.IsActive)
                .Where(a => !_context.UserAchievements.Any(ua =>
                    ua.UserId == userId &&
                    ua.AchievementId == a.AchievementId &&
                    ua.IsUnlocked))
                .ToListAsync();

            foreach (var achievement in pendingAchievements)
            {
                var currentProgress = await CalculateAchievementProgress(userId, achievement);

                var userAchievement = await _context.UserAchievements
                    .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AchievementId == achievement.AchievementId);

                if (userAchievement == null)
                {
                    userAchievement = new UserAchievement
                    {
                        UserId = userId,
                        AchievementId = achievement.AchievementId,
                        CurrentProgress = currentProgress
                    };
                    _context.UserAchievements.Add(userAchievement);
                }
                else
                {
                    userAchievement.CurrentProgress = currentProgress;
                }

                if (currentProgress >= achievement.TargetValue && !userAchievement.IsUnlocked)
                {
                    userAchievement.IsUnlocked = true;
                    userAchievement.UnlockedDate = DateTime.Now;

                    user.Gems = (user.Gems ?? 0) + achievement.RewardGems;
                    user.TotalXp = (user.TotalXp ?? 0) + achievement.RewardXp;

                    unlockedAchievements.Add(new UnlockAchievementResponse
                    {
                        AchievementId = achievement.AchievementId,
                        AchievementName = achievement.AchievementName,
                        Description = achievement.Description,
                        Rarity = achievement.Rarity,
                        RewardGems = achievement.RewardGems,
                        RewardXp = achievement.RewardXp,
                        TotalGems = user.Gems ?? 0,
                        TotalXp = user.TotalXp ?? 0
                    });
                }
            }

            await _context.SaveChangesAsync();
            return unlockedAchievements;
        }

        private async Task<int> CalculateAchievementProgress(int userId, Achievement achievement)
        {
            var parts = achievement.Condition.Split(':');
            if (parts.Length != 2) return 0;

            var conditionType = parts[0];

            return conditionType switch
            {
                "CompleteLesson" => await _context.UserLessonProgresses
                    .CountAsync(ulp => ulp.UserId == userId && ulp.IsCompleted == true),
                "LearnKanji" => await _context.UserKanjiProgresses
                    .CountAsync(ukp => ukp.UserId == userId && ukp.IsLearned == true),
                "TotalXp" => await _context.Users
                    .Where(u => u.UserId == userId)
                    .Select(u => u.TotalXp ?? 0)
                    .FirstOrDefaultAsync(),
                "Streak" => await _context.Users
                    .Where(u => u.UserId == userId)
                    .Select(u => u.StreakCount ?? 0)
                    .FirstOrDefaultAsync(),
                "CollectGems" => await _context.Users
                    .Where(u => u.UserId == userId)
                    .Select(u => u.Gems ?? 0)
                    .FirstOrDefaultAsync(),
                "PerfectScore" => await _context.UserLessonProgresses
                    .CountAsync(ulp => ulp.UserId == userId && ulp.Score == 100),
                "ThreeStars" => await _context.UserLessonProgresses
                    .CountAsync(ulp => ulp.UserId == userId && ulp.Stars == 3),
                "CompleteTopic" => await _context.UserLessonProgresses
                    .Where(ulp => ulp.UserId == userId && ulp.IsCompleted == true)
                    .Select(ulp => ulp.Lesson.TopicId)
                    .Distinct()
                    .CountAsync(),
                _ => 0
            };
        }
    }
}
