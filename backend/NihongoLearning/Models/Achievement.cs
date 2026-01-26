namespace NihongoLearning.Models
{
    public partial class Achievement
    {
        public int AchievementId { get; set; }

        public string AchievementName { get; set; } = null!;

        public string? Description { get; set; }

        /// <summary>
        /// Loại achievement: Lesson, Kanji, Streak, Score, Gems, Topic, Special
        /// </summary>
        public string Category { get; set; } = null!;

        /// <summary>
        /// Điều kiện để đạt achievement (VD: CompleteLesson:10, LearnKanji:50, Streak:7)
        /// </summary>
        public string Condition { get; set; } = null!;

        /// <summary>
        /// Giá trị target để hoàn thành (VD: 10, 50, 100)
        /// </summary>
        public int TargetValue { get; set; }

        public string? IconUrl { get; set; }

        /// <summary>
        /// Phần thưởng khi unlock
        /// </summary>
        public int RewardGems { get; set; } = 0;

        public int RewardXp { get; set; } = 0;

        /// <summary>
        /// Độ hiếm: Common, Rare, Epic, Legendary
        /// </summary>
        public string Rarity { get; set; } = "Common";

        public bool IsActive { get; set; } = true;

        public int OrderIndex { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
    }
}
