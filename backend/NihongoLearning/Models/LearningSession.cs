namespace NihongoLearning.Models
{
    public partial class LearningSession
    {
        public int SessionId { get; set; }

        public int UserId { get; set; }

        public int LessonId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int TotalQuestions { get; set; }

        public int CorrectAnswers { get; set; }

        public int Lives { get; set; } = 5;

        public string Status { get; set; } = null!; // InProgress, Completed, Failed

        public virtual User User { get; set; } = null!;

        public virtual Lesson Lesson { get; set; } = null!;
    }
}
