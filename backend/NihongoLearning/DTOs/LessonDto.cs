namespace NihongoLearning.DTOs;

public class LessonDto
{
    public int LessonId { get; set; }
    public string LessonName { get; set; } = null!;
    public string? Description { get; set; }
    public string? LevelRequired { get; set; }
    public int XpReward { get; set; }
    public int GemsReward { get; set; }
    public int? Duration { get; set; }
    public bool IsCompleted { get; set; }
    public int? Score { get; set; }
    public int? Stars { get; set; }
    public bool IsLocked { get; set; } // Khóa nếu chưa hoàn thành bài trước
}