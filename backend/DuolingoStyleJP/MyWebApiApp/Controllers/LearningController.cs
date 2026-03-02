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
    [Route("api/learning")]
    [ApiController]
    public class LearningController : ControllerBase
    {
        //private readonly ILearningRepository _learningService;

        //public LearningController(ILearningRepository learningService)
        //{
        //    _learningService = learningService;
        //}

        [HttpGet("japanese-path")]
        public async Task<IActionResult> GetPath()
        {
            // 1️⃣ Lấy UserId từ JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Token không hợp lệ");
            }

            // 2️⃣ Lấy toàn bộ Lesson
            var lessons = await _context.Lessons
                .OrderBy(l => l.LessonId)
                .ToListAsync();

            // 3️⃣ Lấy danh sách LessonId đã hoàn thành
            var completedLessonIds = await _context.UserProgress
                .Where(p => p.UserId == userId)
                .Select(p => p.LessonId)
                .ToListAsync();

            // 4️⃣ Merge lại
            var result = lessons.Select(lesson => new
            {
                lesson.LessonId,
                lesson.LessonName,
                

                // Nếu tồn tại trong UserProgress => đã hoàn thành
                IsCompleted = completedLessonIds.Contains(lesson.LessonId)
            });

            return Ok(result);
        }
        

        //// Endpoint gọi khi hoàn thành 1 Lesson
        //[HttpPost("complete-lesson/{lessonId}")]
        //public async Task<IActionResult> CompleteLesson(int lessonId)
        //{
        //    var userId = int.Parse(User.FindFirst("UserID")?.Value ?? "1");
        //    await _learningService.UpdateProgressAsync(userId, lessonId);
        //    return Ok(new { message = "Progress updated successfully" });
        //}
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LearningController(
            ApplicationDbContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("my-progress")]
        public async Task<IActionResult> GetMyProgress()
        {
            //// Lấy UserId từ JWT
            //var userId = _userManager.GetUserId(User);

            //if(User.Identity != null && User.Identity.IsAuthenticated)
            //{
            //    var progress = await _context.UserProgress
            //    .Where(p => p.UserId == userId)
            //    .Include(p => p.Lesson)
            //    .Select(p => new
            //    {
            //        p.LessonId,
            //        LessonName = p.Lesson.LessonName,
            //    })
            //    .ToListAsync();
            //    return Ok(progress);
            //}
            //else
            //{
            //    return Unauthorized("Không có dữ liệu tiến độ vì người dùng chưa đăng nhập");
            //}
            // Nếu chưa đăng nhập
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized("Người dùng chưa đăng nhập");
            }

            // Lấy UserId từ token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Token không hợp lệ hoặc không chứa UserId");
            }

            var progress = await _context.UserProgress
                .Where(p => p.UserId == userId)
                .Include(p => p.Lesson)
                .Select(p => new
                {
                    p.LessonId,
                    LessonName = p.Lesson.LessonName,
                    p.CompletedDate,
                    p.EarnedXP
                })
                .ToListAsync();

            return Ok(progress);


        }
    }

}

