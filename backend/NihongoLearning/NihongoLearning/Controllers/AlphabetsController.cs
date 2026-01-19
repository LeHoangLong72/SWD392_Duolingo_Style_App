using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NihongoLearning.DTOs;
using NihongoLearning.Models;

namespace NihongoLearning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlphabetsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AlphabetsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy tất cả Hiragana
    /// </summary>
    [HttpGet("hiragana")]
    public async Task<ActionResult<IEnumerable<AlphabetDto>>> GetHiragana()
    {
        var hiragana = await _context.Alphabets
            .Where(a => a.Type == "hiragana")
            .Select(a => new AlphabetDto
            {
                AlphabetId = a.AlphabetId,
                Character = a.Character,
                Type = a.Type,
                Level = a.Level,
                Meaning = a.Meaning,
                IsLearned = true
            })
            .ToListAsync();

        return Ok(hiragana);
    }

    /// <summary>
    /// Lấy tất cả Katakana
    /// </summary>
    [HttpGet("katakana")]
    public async Task<ActionResult<IEnumerable<AlphabetDto>>> GetKatakana()
    {
        var katakana = await _context.Alphabets
            .Where(a => a.Type == "katakana")
            .Select(a => new AlphabetDto
            {
                AlphabetId = a.AlphabetId,
                Character = a.Character,
                Type = a.Type,
                Level = a.Level,
                Meaning = a.Meaning,
                IsLearned = true
            })
            .ToListAsync();

        return Ok(katakana);
    }

    /// <summary>
    /// Lấy Kanji theo level (N5-N1) cho user cụ thể
    /// Kanji chưa học hiển thị dấu "?"
    /// </summary>
    [HttpGet("kanji")]
    public async Task<ActionResult<IEnumerable<KanjiDto>>> GetKanji(
        [FromQuery] string? level,
        [FromQuery] int userId)
    {
        var validLevels = new[] { "N5", "N4", "N3", "N2", "N1" };
        if (string.IsNullOrEmpty(level) || !validLevels.Contains(level.ToUpper()))
        {
            return BadRequest(new { message = "Level phải là N5, N4, N3, N2 hoặc N1" });
        }

        var kanjiQuery = _context.Alphabets
            .Where(a => a.Type == "kanji" && a.Level == level.ToUpper())
            .AsQueryable();

        var learnedKanjiIds = await _context.UserKanjiProgresses
            .Where(up => up.UserId == userId && up.IsLearned == true)
            .Select(up => up.AlphabetId)
            .ToListAsync();

        var kanjiList = await kanjiQuery
            .Select(k => new
            {
                k.AlphabetId,
                k.Character,
                Level = k.Level ?? string.Empty,
                k.Meaning
            })
            .ToListAsync();

        var result = kanjiList.Select(k => new KanjiDto
        {
            AlphabetId = k.AlphabetId,
            Character = learnedKanjiIds.Contains(k.AlphabetId) ? k.Character : "?",
            Level = k.Level,
            Meaning = learnedKanjiIds.Contains(k.AlphabetId) ? k.Meaning : null,
            IsLearned = learnedKanjiIds.Contains(k.AlphabetId)
        });

        return Ok(result);
    }

    /// <summary>
    /// Lấy chi tiết 1 ký tự theo ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<AlphabetDto>> GetAlphabetById(int id, [FromQuery] int? userId = null)
    {
        var alphabet = await _context.Alphabets.FindAsync(id);

        if (alphabet == null)
        {
            return NotFound(new { message = "Không tìm thấy ký tự" });
        }

        bool isLearned = true;
        if (userId.HasValue && alphabet.Type == "kanji")
        {
            isLearned = await _context.UserKanjiProgresses
                .AnyAsync(up => up.UserId == userId.Value
                    && up.AlphabetId == id
                    && up.IsLearned == true);
        }

        var dto = new AlphabetDto
        {
            AlphabetId = alphabet.AlphabetId,
            Character = (alphabet.Type == "kanji" && !isLearned) ? "?" : alphabet.Character,
            Type = alphabet.Type,
            Level = alphabet.Level,
            Meaning = (alphabet.Type == "kanji" && !isLearned) ? null : alphabet.Meaning,
            IsLearned = isLearned
        };

        return Ok(dto);
    }

    /// <summary>
    /// Lấy tất cả ký tự
    /// </summary>
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<AlphabetDto>>> GetAllAlphabets()
    {
        var alphabets = await _context.Alphabets
            .Select(a => new AlphabetDto
            {
                AlphabetId = a.AlphabetId,
                Character = a.Character,
                Type = a.Type,
                Level = a.Level,
                Meaning = a.Meaning,
                IsLearned = a.Type != "kanji"
            })
            .ToListAsync();

        return Ok(alphabets);
    }
}