namespace NihongoLearning.DTOs
{
    public class StartLearningResponse
    {
        public int SessionId { get; set; }
        public string LessonName { get; set; } = null!;
        public int TotalQuestions { get; set; }
        public int Lives { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
    }
}
