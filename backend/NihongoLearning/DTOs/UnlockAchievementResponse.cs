namespace NihongoLearning.DTOs
{
    public class UnlockAchievementResponse
    {
        public int AchievementId { get; set; }
        public string AchievementName { get; set; } = null!;
        public string? Description { get; set; }
        public string Rarity { get; set; } = null!;
        public int RewardGems { get; set; }
        public int RewardXp { get; set; }
        public int TotalGems { get; set; }
        public int TotalXp { get; set; }
        public string Message { get; set; } = "🎉 Chúc mừng! Bạn đã đạt được thành tựu mới!";
    }
}
