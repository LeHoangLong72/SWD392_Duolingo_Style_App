namespace NihongoLearning.DTOs
{
    public class UserProfileDto
    {
        public int UserID { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
        public int XP { get; set; }
        public int Gems { get; set; }
        public int StreakCount { get; set; }
    }
}
