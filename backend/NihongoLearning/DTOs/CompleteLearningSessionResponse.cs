namespace NihongoLearning.DTOs
{
    public class CompleteLearningSessionResponse
    {
        public int Score { get; set; }
        public int Stars { get; set; }
        public int XpEarned { get; set; }
        public int GemsEarned { get; set; }
        public int TotalXp { get; set; }
        public int TotalGems { get; set; }
        public bool IsNewRecord { get; set; }
        public string Message { get; set; } = null!;
    }
}
