using System;
using System.Collections.Generic;

namespace NihongoLearning.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public int? Gems { get; set; }

    public int? TotalXp { get; set; }

    public int? StreakCount { get; set; }

    public DateTime? LastLearnedDate { get; set; }

    public virtual ICollection<UserKanjiProgress> UserKanjiProgresses { get; set; } = new List<UserKanjiProgress>();
}
