namespace NihongoLearning.DTOs;

public class CompleteLessonResponse
{
    public int XpEarned { get; set; }
    public int GemsEarned { get; set; }
    public int Stars { get; set; } // 1-3 sao
    public bool IsNewRecord { get; set; }
    public int TotalXp { get; set; }
    public int TotalGems { get; set; }
    public string Message { get; set; } = string.Empty;
}