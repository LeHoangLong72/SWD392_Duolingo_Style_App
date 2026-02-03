namespace NihongoLearning.Models
{
    public class Node
    {
        public int NodeId { get; set; }
        public int UnitId { get; set; }
        public int UserId { get; set; }
        public string NodeType { get; set; }
        public int Position { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
