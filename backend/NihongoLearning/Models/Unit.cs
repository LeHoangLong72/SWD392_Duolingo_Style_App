namespace NihongoLearning.Models
{
    public class Unit
    {
        public int UnitId { get; set; }
        public int UnitNumber { get; set; }
        public string Title { get; set; }
        public List<Node> Nodes { get; set; }
    }
}
