using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NihongoLearning.DTOs;
using NihongoLearning.Models;

namespace NihongoLearning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<LessonsController> _logger;

    public LessonsController(AppDbContext context, ILogger<LessonsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Lấy tất cả bài học của 1 chủ đề
    /// </summary>
    [HttpGet("topic/{topicId}")]
    public async Task<ActionResult<IEnumerable<LessonDto>>> GetLessonsByTopic(int topicId, [FromQuery] int userId)
    {
        // Kiểm tra topic tồn tại
        var topicExists = await _context.Topics.AnyAsync(t => t.TopicId == topicId);
        if (!topicExists)
        {
            return NotFound(new { message = "Không tìm thấy chủ đề" });
        }

        // Lấy danh sách bài học
        var lessons = await _context.Lessons
            .Where(l => l.TopicId == topicId && l.IsActive == true)
            .OrderBy(l => l.OrderIndex)
            .Select(l => new
            {
                l.LessonId,
                l.LessonName,
                l.Description,
                l.LevelRequired,
                l.XpReward,
                l.GemsReward,
                l.Duration,
                l.OrderIndex
            })
            .ToListAsync();

        // Lấy tiến độ của user
        var userProgress = await _context.UserLessonProgresses
            .Where(up => up.UserId == userId
                && lessons.Select(l => l.LessonId).Contains(up.LessonId))
            .ToDictionaryAsync(up => up.LessonId);

        // Build result với logic khóa/mở bài
        var result = new List<LessonDto>();
        bool previousCompleted = true;

        foreach (var lesson in lessons.OrderBy(l => l.OrderIndex))
        {
            var progress = userProgress.ContainsKey(lesson.LessonId)
                ? userProgress[lesson.LessonId]
                : null;

            result.Add(new LessonDto
            {
                LessonId = lesson.LessonId,
                LessonName = lesson.LessonName,
                Description = lesson.Description,
                LevelRequired = lesson.LevelRequired,
                XpReward = lesson.XpReward ?? 10,
                GemsReward = lesson.GemsReward ?? 5,
                Duration = lesson.Duration,
                IsCompleted = progress?.IsCompleted ?? false,
                Score = progress?.Score,
                Stars = progress?.Stars,
                IsLocked = !previousCompleted
            });

            previousCompleted = progress?.IsCompleted ?? false;
        }

        return Ok(result);
    }

    /// <summary>
    /// Lấy chi tiết 1 bài học (bao gồm các ký tự cần học)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<LessonDetailDto>> GetLessonById(int id)
    {
        var lesson = await _context.Lessons
            .Include(l => l.LessonContents)
            .ThenInclude(lc => lc.Alphabet)
            .FirstOrDefaultAsync(l => l.LessonId == id && l.IsActive == true);

        if (lesson == null)
        {
            return NotFound(new { message = "Không tìm thấy bài học" });
        }

        var result = new LessonDetailDto
        {
            LessonId = lesson.LessonId,
            LessonName = lesson.LessonName,
            Description = lesson.Description,
            LevelRequired = lesson.LevelRequired,
            XpReward = lesson.XpReward ?? 10,
            GemsReward = lesson.GemsReward ?? 5,
            Characters = lesson.LessonContents
                .OrderBy(lc => lc.OrderIndex)
                .Select(lc => new AlphabetDto
                {
                    AlphabetId = lc.Alphabet!.AlphabetId,
                    Character = lc.Alphabet.Character,
                    Type = lc.Alphabet.Type,
                    Level = lc.Alphabet.Level,
                    Meaning = lc.Alphabet.Meaning,
                    IsLearned = false
                })
                .ToList()
        };

        return Ok(result);
    }

    /// <summary>
    /// Hoàn thành bài học
    /// </summary>
    [HttpPost("{id}/complete")]
    public async Task<ActionResult<CompleteLessonResponse>> CompleteLesson(
        int id,
        [FromBody] CompleteLessonRequest request)
    {
        if (request.LessonId != id)
        {
            return BadRequest(new { message = "LessonId không khớp" });
        }

        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
        {
            return NotFound(new { message = "Không tìm thấy user" });
        }

        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson == null)
        {
            return NotFound(new { message = "Không tìm thấy bài học" });
        }

        // Tính số sao
        int stars = request.Score switch
        {
            >= 90 => 3,
            >= 70 => 2,
            >= 50 => 1,
            _ => 0
        };

        // Kiểm tra đã làm bài này chưa
        var existingProgress = await _context.UserLessonProgresses
            .FirstOrDefaultAsync(up => up.UserId == request.UserId && up.LessonId == id);

        bool isNewRecord = false;

        if (existingProgress == null)
        {
            existingProgress = new UserLessonProgress
            {
                UserId = request.UserId,
                LessonId = id,
                IsCompleted = true,
                Score = request.Score,
                Stars = stars,
                CompletedDate = DateTime.Now
            };
            _context.UserLessonProgresses.Add(existingProgress);
            isNewRecord = true;
        }
        else
        {
            if (request.Score > (existingProgress.Score ?? 0))
            {
                existingProgress.Score = request.Score;
                existingProgress.Stars = stars;
                existingProgress.CompletedDate = DateTime.Now;
                isNewRecord = true;
            }
        }

        int xpEarned = 0;
        int gemsEarned = 0;

        if (isNewRecord)
        {
            xpEarned = lesson.XpReward ?? 10;
            gemsEarned = lesson.GemsReward ?? 5;

            if (request.Score >= 90)
            {
                xpEarned += 5;
                gemsEarned += 2;
            }
            else if (request.Score >= 70)
            {
                xpEarned += 3;
                gemsEarned += 1;
            }

            user.TotalXp = (user.TotalXp ?? 0) + xpEarned;
            user.Gems = (user.Gems ?? 0) + gemsEarned;
            user.LastLearnedDate = DateTime.Now;
        }

        await _context.SaveChangesAsync();

        var response = new CompleteLessonResponse
        {
            XpEarned = xpEarned,
            GemsEarned = gemsEarned,
            Stars = stars,
            IsNewRecord = isNewRecord,
            TotalXp = user.TotalXp ?? 0,
            TotalGems = user.Gems ?? 0,
            Message = stars switch
            {
                3 => "🌟 Xuất sắc! Hoàn hảo 100%!",
                2 => "⭐ Tốt lắm! Tiếp tục phát huy!",
                1 => "✨ Cố gắng thêm nhé!",
                _ => "Hãy thử lại để đạt điểm cao hơn!"
            }
        };

        _logger.LogInformation("User {UserId} hoàn thành bài {LessonId} với {Score} điểm, {Stars} sao",
            request.UserId, id, request.Score, stars);

        return Ok(response);
    }
}