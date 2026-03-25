using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.Interfaces;
using MyWebApiApp.Models;
using System.Security.Claims;

namespace MyWebApiApp.Controllers
{
    [Route("api/learning-path")]
    [ApiController]
    public class LearningPathController : ControllerBase
    {
        private readonly ILearningPathRepository _learningPathRepo;

        public LearningPathController(ILearningPathRepository learningPathRepo)
        {
            _learningPathRepo = learningPathRepo;
        }


        [HttpGet("japanese-path")]
        public async Task<IActionResult> GetPath()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Token không hợp lệ");

            var lessons = await _learningPathRepo.GetAllLessonsAsync();
            var completedIds = await _learningPathRepo.GetCompletedLessonIdsAsync(userId);

            var result = lessons.Select(l => new
            {
                l.LessonId,
                l.LessonName,
                IsCompleted = completedIds.Contains(l.LessonId)
            });

            return Ok(result);
        }
        
        

        [HttpGet("my-progress")]
        public async Task<IActionResult> GetMyProgress()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Token không hợp lệ");

            var progress = await _learningPathRepo.GetUserProgressAsync(userId);

            var result = progress.Select(p => new
            {
                p.LessonId,
                LessonName = p.Lesson.LessonName,
                p.CompletedDate,
                p.EarnedXP
            });

            return Ok(result);


        }

        [HttpGet("mistakes")]
        public async Task<IActionResult> GetMistakesAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Token không hợp lệ");

            var mistakes = await _learningPathRepo.GetUserMistakesAsync(userId);

            var result = mistakes.Select(x => new
            {
                x.QuestionId,
                x.WrongCount,
                x.LastWrongAt,
                Question = x.Question.Content,
                Options = x.Question.QuestionOptions.Select(o => new
                {
                    o.OptionId,
                    o.OptionText
                })
            });

            return Ok(result);
        }
    }

}

