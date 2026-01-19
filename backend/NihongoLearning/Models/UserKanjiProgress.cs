using System;
using System.Collections.Generic;

namespace NihongoLearning.Models;

public partial class UserKanjiProgress
{
    public int ProgressId { get; set; }

    public int? UserId { get; set; }

    public int? AlphabetId { get; set; }

    public bool? IsLearned { get; set; }

    public DateTime? LearnedDate { get; set; }

    public virtual Alphabet? Alphabet { get; set; }

    public virtual User? User { get; set; }
}
