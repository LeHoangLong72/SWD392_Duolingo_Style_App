namespace NihongoLearning.DTOs;

public class KanjiDto
{
    public int AlphabetId { get; set; }
    public string Character { get; set; } = "?";
    public string? Level { get; set; }
    public string? Meaning { get; set; }
    public bool IsLearned { get; set; }
}