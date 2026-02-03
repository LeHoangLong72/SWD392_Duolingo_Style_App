using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NihongoLearning.Models;

[Table("Users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    public string Role { get; set; } = "User";
    public int TotalXP { get; set; }
    public int HeartCount { get; set; }
    public int CurrentGems { get; set; }
    public int StreakCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public UserProfile UserProfile { get; set; }
    
}