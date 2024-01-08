namespace Infraestructure.Entity.Entities.Payments
{
    public partial class QuestionAnswer
    {
        public Guid Id { get; set; }
        public Guid? QuestionId { get; set; }
        public Guid? ProfileId { get; set; }
        public string? Answer { get; set; }
        public bool? Deleted { get; set; }
    }
}