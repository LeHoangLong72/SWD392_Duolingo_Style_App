namespace NihongoLearning.DTOs;

public class LearnedCharacterDto
{
    public int AlphabetId { get; set; }
    public string Character { get; set; } = null!;
    public string? Type { get; set; }
    public string? Level { get; set; }
    public string? Meaning { get; set; }
    public DateTime LearnedDate { get; set; }
}