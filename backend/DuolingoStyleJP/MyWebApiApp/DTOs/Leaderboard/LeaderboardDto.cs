namespace MyWebApiApp.DTOs.Leaderboard
{
    public class LeaderboardDto
    {
        public int Rank { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public int TotalXP { get; set; }
        public int Level { get; set; }
    }
}
