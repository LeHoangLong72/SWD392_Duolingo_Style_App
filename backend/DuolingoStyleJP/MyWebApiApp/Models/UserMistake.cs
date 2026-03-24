using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApiApp.Models
{
    [Table("UserMistakes")]
    public class UserMistake
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int QuestionId { get; set; }
        public int WrongCount { get; set; }
        public DateTime LastWrongAt { get; set; }
        public Question Question { get; set; }
    }
}
