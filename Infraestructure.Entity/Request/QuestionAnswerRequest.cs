namespace Infraestructure.Entity.Request
{
    public class QuestionAnswerRequest
    {
        public Guid userId { get; set; }
        public string answer { get; set; }
    }
}