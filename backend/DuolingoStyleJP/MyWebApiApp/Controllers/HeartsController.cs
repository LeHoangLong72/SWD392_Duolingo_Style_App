using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.DTOs.LessonContent;
using MyWebApiApp.Interfaces;
using System.Security.Claims;

namespace MyWebApiApp.Controllers
{
    [Route("api/hearts")]
    [ApiController]
    public class HeartsController : ControllerBase
    {
        private readonly IHeartRepository _heartRepo;
        private readonly ApplicationDbContext _context;

        public HeartsController(IHeartRepository heartRepo, ApplicationDbContext context)
        {
            _heartRepo = heartRepo;
            _context = context;
        }
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet]
        public async Task<IActionResult> GetHearts()
        {
            var userId = GetUserId();
            var hearts = await _heartRepo.GetHeartsAsync(userId);
            if (hearts == null)
            {
                return NotFound();
            }
            return Ok(hearts);
        }


        [HttpPost("practice")]
        public async Task<IActionResult> PracticeMistake([FromBody] SubmitAnswerRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var option = await _context.QuestionOptions
                .FirstOrDefaultAsync(o =>
                    o.OptionId == request.SelectedOptionId &&
                    o.QuestionId == request.QuestionId);

            if (option == null)
                return BadRequest("Invalid option");

            var isCorrect = option.IsCorrect;

            if (isCorrect)
            {
                // ✅ Xóa khỏi danh sách sai
                var mistake = await _context.UserMistakes
                    .FirstOrDefaultAsync(x =>
                        x.UserId == userId &&
                        x.QuestionId == request.QuestionId);

                if (mistake != null)
                {
                    _context.UserMistakes.Remove(mistake);
                }

                // ❤️ Hồi tim
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (user.CurrentHearts < user.MaxHearts)
                {
                    user.CurrentHearts++;
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                isCorrect,
                message = isCorrect ? "Correct! +1 heart ❤️" : "Sai rồi!"
            });
        }

    }
}
