using System;
using System.Collections.Generic;

namespace NihongoLearning.Models;

public class Alphabet
{
    public int AlphabetId { get; set; }
    public string Character { get; set; } = null!;
    public string Type { get; set; }
    public string Level { get; set; }
    public string Meaning { get; set; }
}
