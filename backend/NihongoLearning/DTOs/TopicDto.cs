namespace NihongoLearning.DTOs;

public class TopicDto
{
    public int TopicId { get; set; }
    public string TopicName { get; set; } = null!;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public int TotalLessons { get; set; }
    public int CompletedLessons { get; set; }
    public int ProgressPercentage { get; set; }
}