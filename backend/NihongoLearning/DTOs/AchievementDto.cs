namespace NihongoLearning.DTOs
{
    public class AchievementDto
    {
        public int AchievementId { get; set; }
        public string AchievementName { get; set; } = null!;
        public string? Description { get; set; }
        public string Category { get; set; } = null!;
        public int TargetValue { get; set; }
        public string? IconUrl { get; set; }
        public int RewardGems { get; set; }
        public int RewardXp { get; set; }
        public string Rarity { get; set; } = "Common";

        // Thông tin progress của user (nếu có)
        public int CurrentProgress { get; set; }
        public bool IsUnlocked { get; set; }
        public DateTime? UnlockedDate { get; set; }
        public int ProgressPercentage { get; set; }
    }
}
