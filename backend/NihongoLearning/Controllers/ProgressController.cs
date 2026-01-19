using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NihongoLearning.DTOs;
using NihongoLearning.Models;

namespace NihongoLearning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgressController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProgressController> _logger;

    public ProgressController(AppDbContext context, ILogger<ProgressController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Đánh dấu đã học 1 ký tự (unlock Kanji)
    /// </summary>
    [HttpPost("learn-character")]
    public async Task<ActionResult<LearnedCharacterDto>> LearnCharacter([FromBody] LearnCharacterRequest request)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
        {
            return NotFound(new { message = "Không tìm thấy user" });
        }

        var alphabet = await _context.Alphabets.FindAsync(request.AlphabetId);
        if (alphabet == null)
        {
            return NotFound(new { message = "Không tìm thấy ký tự" });
        }

        var existingProgress = await _context.UserKanjiProgresses
            .FirstOrDefaultAsync(up => up.UserId == request.UserId && up.AlphabetId == request.AlphabetId);

        if (existingProgress != null && existingProgress.IsLearned == true)
        {
            return BadRequest(new { message = "Ký tự này đã được học rồi" });
        }

        if (existingProgress == null)
        {
            existingProgress = new UserKanjiProgress
            {
                UserId = request.UserId,
                AlphabetId = request.AlphabetId,
                IsLearned = true,
                LearnedDate = DateTime.Now
            };
            _context.UserKanjiProgresses.Add(existingProgress);
        }
        else
        {
            existingProgress.IsLearned = true;
            existingProgress.LearnedDate = DateTime.Now;
        }

        await _context.SaveChangesAsync();

        var result = new LearnedCharacterDto
        {
            AlphabetId = alphabet.AlphabetId,
            Character = alphabet.Character,
            Type = alphabet.Type,
            Level = alphabet.Level,
            Meaning = alphabet.Meaning,
            LearnedDate = existingProgress.LearnedDate ?? DateTime.Now
        };

        _logger.LogInformation("User {UserId} học ký tự {Character} ({Type})",
            request.UserId, alphabet.Character, alphabet.Type);

        return Ok(result);
    }

    /// <summary>
    /// Hoàn thành bài học - Tính XP, Gems, Streak bonus
    /// </summary>
    [HttpPost("complete-lesson")]
    public async Task<ActionResult<LessonRewardResponse>> CompleteLesson([FromBody] CompleteLessonRequest request)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
        {
            return NotFound(new { message = "Không tìm thấy user" });
        }

        int baseXp = 10;
        int baseGems = 5;

        if (request.Score >= 90)
        {
            baseXp += 5;
            baseGems += 2;
        }
        else if (request.Score >= 70)
        {
            baseXp += 3;
            baseGems += 1;
        }

        bool streakBonusApplied = false;
        int bonusXp = 0;
        int bonusGems = 0;

        var today = DateTime.Today;
        var lastLearned = user.LastLearnedDate?.Date;

        if (lastLearned.HasValue)
        {
            var daysDiff = (today - lastLearned.Value).Days;

            if (daysDiff == 0)
            {
                bonusXp = 2;
                bonusGems = 1;
                streakBonusApplied = true;
            }
            else if (daysDiff == 1)
            {
                user.StreakCount = (user.StreakCount ?? 0) + 1;
                bonusXp = 5 + (user.StreakCount ?? 0);
                bonusGems = 2 + (user.StreakCount ?? 0) / 3;
                streakBonusApplied = true;
            }
            else if (daysDiff > 1)
            {
                user.StreakCount = 1;
            }
        }
        else
        {
            user.StreakCount = 1;
        }

        int totalXpEarned = baseXp + bonusXp;
        int totalGemsEarned = baseGems + bonusGems;

        user.TotalXp = (user.TotalXp ?? 0) + totalXpEarned;
        user.Gems = (user.Gems ?? 0) + totalGemsEarned;
        user.LastLearnedDate = DateTime.Now;

        await _context.SaveChangesAsync();

        var response = new LessonRewardResponse
        {
            XpEarned = baseXp,
            GemsEarned = baseGems,
            BonusXp = bonusXp,
            BonusGems = bonusGems,
            TotalXp = user.TotalXp ?? 0,
            TotalGems = user.Gems ?? 0,
            CurrentStreak = user.StreakCount ?? 0,
            StreakBonusApplied = streakBonusApplied,
            Message = streakBonusApplied
                ? $"🔥 Streak {user.StreakCount} ngày! Bonus +{bonusXp} XP, +{bonusGems} Gems!"
                : "Hoàn thành bài học!"
        };

        _logger.LogInformation("User {UserId} hoàn thành bài {LessonId}. XP: +{XP}, Gems: +{Gems}, Streak: {Streak}",
            request.UserId, request.LessonId, totalXpEarned, totalGemsEarned, user.StreakCount);

        return Ok(response);
    }

    /// <summary>
    /// Lấy tiến độ học của user
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<UserProgressDto>> GetUserProgress(int userId)
    {
        var user = await _context.Users
            .Include(u => u.UserKanjiProgresses)
            .ThenInclude(up => up.Alphabet)
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
        {
            return NotFound(new { message = "Không tìm thấy user" });
        }

        var learnedCharacters = user.UserKanjiProgresses
            .Where(up => up.IsLearned == true && up.Alphabet != null)
            .OrderByDescending(up => up.LearnedDate)
            .Take(10)
            .Select(up => new LearnedCharacterDto
            {
                AlphabetId = up.Alphabet!.AlphabetId,
                Character = up.Alphabet.Character,
                Type = up.Alphabet.Type,
                Level = up.Alphabet.Level,
                Meaning = up.Alphabet.Meaning,
                LearnedDate = up.LearnedDate ?? DateTime.Now
            })
            .ToList();

        var totalKanji = user.UserKanjiProgresses
            .Count(up => up.IsLearned == true && up.Alphabet != null && up.Alphabet.Type == "kanji");

        var result = new UserProgressDto
        {
            UserId = user.UserId,
            Username = user.Username,
            TotalXp = user.TotalXp ?? 0,
            Gems = user.Gems ?? 0,
            StreakCount = user.StreakCount ?? 0,
            LastLearnedDate = user.LastLearnedDate,
            TotalKanjiLearned = totalKanji,
            TotalCharactersLearned = user.UserKanjiProgresses.Count(up => up.IsLearned == true),
            RecentlyLearned = learnedCharacters
        };

        return Ok(result);
    }
}