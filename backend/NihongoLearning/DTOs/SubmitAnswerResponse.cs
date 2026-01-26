namespace NihongoLearning.DTOs
{
    public class SubmitAnswerResponse
    {
        public bool IsCorrect { get; set; }
        public int CorrectOptionId { get; set; }
        public string CorrectAnswer { get; set; } = null!;
        public string? Explanation { get; set; }
        public int CurrentLives { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public int ProgressPercentage { get; set; }
        public bool SessionCompleted { get; set; }
        public bool SessionFailed { get; set; }
    }
}
