namespace NihongoLearning.DTOs;

public class LessonDetailDto
{
    public int LessonId { get; set; }
    public string LessonName { get; set; } = null!;
    public string? Description { get; set; }
    public string? LevelRequired { get; set; }
    public int XpReward { get; set; }
    public int GemsReward { get; set; }
    public List<AlphabetDto> Characters { get; set; } = new();
}