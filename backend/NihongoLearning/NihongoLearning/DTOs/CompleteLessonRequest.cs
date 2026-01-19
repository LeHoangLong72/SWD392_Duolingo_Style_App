namespace NihongoLearning.DTOs;

public class CompleteLessonRequest
{
    public int UserId { get; set; }
    public int LessonId { get; set; }
    public int Score { get; set; }
}