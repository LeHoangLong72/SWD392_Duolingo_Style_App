namespace NihongoLearning.Models
{
    public class UserLessonProgress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NodeId { get; set; }
        public string Status { get; set; }
        public int CurrentLessonIndex { get; set; }
    }
}
