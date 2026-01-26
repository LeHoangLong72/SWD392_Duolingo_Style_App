namespace NihongoLearning.Models
{
    public partial class Question
    {
        public int QuestionId { get; set; }

        public int LessonId { get; set; }

        /// <summary>
        /// Loại câu hỏi: MultipleChoice, Matching, FillBlank, Listening, Speaking
        /// </summary>
        public string QuestionType { get; set; } = null!;

        public string QuestionText { get; set; } = null!;

        /// <summary>
        /// Audio URL cho bài tập nghe
        /// </summary>
        public string? AudioUrl { get; set; }

        /// <summary>
        /// Image URL cho bài tập hình ảnh
        /// </summary>
        public string? ImageUrl { get; set; }

        public int OrderIndex { get; set; }

        public int Points { get; set; } = 10;

        public virtual Lesson Lesson { get; set; } = null!;

        public virtual ICollection<QuestionOption> QuestionOptions { get; set; }
    }
}
