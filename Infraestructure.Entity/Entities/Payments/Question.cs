namespace Infraestructure.Entity.Entities.Payments
{
    public partial class Question
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public bool? Deleted { get; set; }
    }
}