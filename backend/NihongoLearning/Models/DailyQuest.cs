using System;
using System.Collections.Generic;

namespace NihongoLearning.Models;

public partial class DailyQuest
{
    public int QuestId { get; set; }

    public string? QuestName { get; set; }

    public int? RequiredXp { get; set; }

    public int? RewardGems { get; set; }
}
