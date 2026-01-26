namespace NihongoLearning.DTOs
{
    public class SubmitAnswerRequest
    {
        public int SessionId { get; set; }
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
    }
}
