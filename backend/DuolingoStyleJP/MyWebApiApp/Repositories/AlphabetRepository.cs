using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.DTOs.Alphabet;
using MyWebApiApp.Interfaces;
using MyWebApiApp.Models;

namespace MyWebApiApp.Repository
{
    public class AlphabetRepository : IAlphabetRepository
    {
        private readonly ApplicationDbContext _context;

        public AlphabetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AlphabetResponse>> GetAllHiraganaAsync()
        {
            return await _context.Alphabets
                .Where(a => a.Type.ToLower() == "hiragana")
                .Select(s => new AlphabetResponse
                {
                    AlphabetId = s.AlphabetId,
                    Character = s.Character,
                    Type = s.Type,
                    Level = s.Level,
                    Meaning = s.Meaning
                })
                .ToListAsync();
        }

        public async Task<List<AlphabetResponse>> GetAllKanjiAsync(string? level)
        {
            var query = _context.Alphabets
                .Where(a => a.Type.ToLower() == "kanji");
            if (!string.IsNullOrEmpty(level))
            {
                query = query.Where(a => a.Level.ToLower() == level.ToLower());
            }
            return await query.Select(s => new AlphabetResponse
            {
                AlphabetId = s.AlphabetId,
                Character = s.Character,
                Type = s.Type,
                Level = s.Level,
                Meaning = s.Meaning
            }
            ).ToListAsync();
        }

        public async Task<List<AlphabetResponse>> GetAllKatakataAsync()
        {
            return await _context.Alphabets
                .Where(a => a.Type.ToLower() == "katakana")
                .Select(s => new AlphabetResponse
                {
                    AlphabetId = s.AlphabetId,
                    Character = s.Character,
                    Type = s.Type,
                    Level = s.Level,
                    Meaning = s.Meaning
                })
                .ToListAsync();
        }
    }
}
