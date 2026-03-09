namespace MyWebApiApp.DTOs.Profile
{
    public class ProfileResponse
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public int Level { get; set; }

        public int CurrentXP { get; set; }

        public int TotalXP { get; set; }

        public int Gems { get; set; }

        public int Hearts { get; set; }

        public int MaxHearts { get; set; }

        public int CurrentStreak { get; set; }

        public int LongestStreak { get; set; }
    }
}
