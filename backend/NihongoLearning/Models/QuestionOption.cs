namespace NihongoLearning.Models
{
    public partial class QuestionOption
    {
        public int OptionId { get; set; }

        public int QuestionId { get; set; }

        public string OptionText { get; set; } = null!;

        public bool IsCorrect { get; set; }

        public int OrderIndex { get; set; }

        public virtual Question Question { get; set; } = null!;
    }
}
