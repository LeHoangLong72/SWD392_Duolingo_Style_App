namespace NihongoLearning.Models
{
    public class UserAchievement
    {
        public int UserAchievementId { get; set; }

        public int UserId { get; set; }

        public int AchievementId { get; set; }

        /// <summary>
        /// Tiến độ hiện tại (VD: đã hoàn thành 5/10 bài học)
        /// </summary>
        public int CurrentProgress { get; set; } = 0;

        /// <summary>
        /// Đã unlock chưa
        /// </summary>
        public bool IsUnlocked { get; set; } = false;

        /// <summary>
        /// Thời gian unlock
        /// </summary>
        public DateTime? UnlockedDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual User User { get; set; } = null!;

        public virtual Achievement Achievement { get; set; } = null!;
    }
}
