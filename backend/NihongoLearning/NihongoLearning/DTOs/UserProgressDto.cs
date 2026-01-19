namespace NihongoLearning.DTOs;

public class UserProgressDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public int TotalXp { get; set; }
    public int Gems { get; set; }
    public int StreakCount { get; set; }
    public DateTime? LastLearnedDate { get; set; }
    public int TotalKanjiLearned { get; set; }
    public int TotalCharactersLearned { get; set; }
    public List<LearnedCharacterDto> RecentlyLearned { get; set; } = new();
}