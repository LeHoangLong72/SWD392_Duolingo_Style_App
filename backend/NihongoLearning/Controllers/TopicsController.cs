using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NihongoLearning.DTOs;
using NihongoLearning.Models;

namespace NihongoLearning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TopicsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy tất cả chủ đề
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TopicDto>>> GetAllTopics([FromQuery] int? userId = null)
    {
        var topics = await _context.Topics
            .Where(t => t.IsActive == true)
            .OrderBy(t => t.OrderIndex)
            .Select(t => new TopicDto
            {
                TopicId = t.TopicId,
                TopicName = t.TopicName,
                Description = t.Description,
                IconUrl = t.IconUrl,
                TotalLessons = t.Lessons.Count(l => l.IsActive == true),
                CompletedLessons = 0,
                ProgressPercentage = 0
            })
            .ToListAsync();

        return Ok(topics);
    }

    /// <summary>
    /// Lấy chi tiết 1 chủ đề
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TopicDto>> GetTopicById(int id)
    {
        var topic = await _context.Topics
            .Where(t => t.TopicId == id && t.IsActive == true)
            .Select(t => new TopicDto
            {
                TopicId = t.TopicId,
                TopicName = t.TopicName,
                Description = t.Description,
                IconUrl = t.IconUrl,
                TotalLessons = t.Lessons.Count(l => l.IsActive == true),
                CompletedLessons = 0,
                ProgressPercentage = 0
            })
            .FirstOrDefaultAsync();

        if (topic == null)
        {
            return NotFound(new { message = "Không tìm thấy chủ đề" });
        }

        return Ok(topic);
    }
}