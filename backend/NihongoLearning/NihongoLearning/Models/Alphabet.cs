using System;
using System.Collections.Generic;

namespace NihongoLearning.Models;

public partial class Alphabet
{
    public int AlphabetId { get; set; }

    public string Character { get; set; } = null!;

    public string? Type { get; set; }

    public string? Level { get; set; }

    public string? Meaning { get; set; }

    public virtual ICollection<UserKanjiProgress> UserKanjiProgresses { get; set; } = new List<UserKanjiProgress>();
}
