namespace NihongoLearning.DTOs
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionType { get; set; } = null!;
        public string QuestionText { get; set; } = null!;
        public string? AudioUrl { get; set; }
        public string? ImageUrl { get; set; }
        public List<OptionDto> Options { get; set; } = new();
    }
}
