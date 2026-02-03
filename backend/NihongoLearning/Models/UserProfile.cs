using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NihongoLearning.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfileID { get; set; }
        public int UserID { get; set; }
        public string DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
        public int XP { get; set; }
        public int Gems { get; set; }
        public int StreakCount { get; set; }

        public User User { get; set; }
    }
}
