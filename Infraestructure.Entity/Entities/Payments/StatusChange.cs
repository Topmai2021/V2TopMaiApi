namespace Infraestructure.Entity.Entities.Payments
{
    public partial class StatusChange
    {
        public Guid Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Deleted { get; set; }
        public Guid? SellId { get; set; }
        public int StatusId { get; set; }

        public virtual Status Status { get; set; } = null!;
    }
}