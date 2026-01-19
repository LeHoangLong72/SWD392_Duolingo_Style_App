namespace NihongoLearning.DTOs;

public class LessonRewardResponse
{
    public int XpEarned { get; set; }
    public int GemsEarned { get; set; }
    public int BonusXp { get; set; }
    public int BonusGems { get; set; }
    public int TotalXp { get; set; }
    public int TotalGems { get; set; }
    public int CurrentStreak { get; set; }
    public bool StreakBonusApplied { get; set; }
    public string Message { get; set; } = string.Empty;
}