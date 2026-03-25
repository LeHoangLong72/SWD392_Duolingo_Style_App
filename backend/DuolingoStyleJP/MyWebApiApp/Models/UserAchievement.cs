namespace MyWebApiApp.Models
{
    public class UserAchievement
    {
        public int UserAchievementId { get; set; }

        public string UserId { get; set; }

        public int AchievementId { get; set; }

        public DateTime UnlockedAt { get; set; }
        public bool IsClaimed { get; set; } = false;

        public Achievement Achievement { get; set; }

    }
}
