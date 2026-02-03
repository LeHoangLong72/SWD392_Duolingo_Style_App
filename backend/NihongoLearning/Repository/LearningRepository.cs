using Microsoft.EntityFrameworkCore;
using NihongoLearning.Data;
using NihongoLearning.Services;

namespace NihongoLearning.Repository
{
    public class LearningRepository : ILearningService
    {
        private readonly AppDbContext _context;

        public LearningRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task<IEnumerable<object>> GetJapanesePathAsync(int userId)
        {

            throw new NotImplementedException();

        }

        public Task UpdateProgressAsync(int userId, int lessonId)
        {
            throw new NotImplementedException();
        }
    }
}
