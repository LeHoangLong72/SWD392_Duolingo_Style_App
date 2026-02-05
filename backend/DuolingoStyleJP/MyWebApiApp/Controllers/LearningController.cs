using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Interfaces;

namespace MyWebApiApp.Controllers
{
    [Route("api/learning")]
    [ApiController]
    public class LearningController : ControllerBase
    {
        private readonly ILearningRepository _learningService;

        public LearningController(ILearningRepository learningService)
        {
            _learningService = learningService;
        }

        [HttpGet("japanese-path")]
        public async Task<IActionResult> GetPath()
        {
            var userId = int.Parse(User.FindFirst("UserID")?.Value ?? "1");
            var path = await _learningService.GetJapanesePathAsync(userId);
            return Ok(path);
        }

        // Endpoint gọi khi hoàn thành 1 Lesson
        [HttpPost("complete-lesson/{lessonId}")]
        public async Task<IActionResult> CompleteLesson(int lessonId)
        {
            var userId = int.Parse(User.FindFirst("UserID")?.Value ?? "1");
            await _learningService.UpdateProgressAsync(userId, lessonId);
            return Ok(new { message = "Progress updated successfully" });
        }
    }
}
