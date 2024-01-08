namespace Infraestructure.Entity.Entities.Payments
{
    public partial class DevolutionStatusChange
    {
        public Guid Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? DevolutionId { get; set; }
        public bool? Deleted { get; set; }
        public int StatusId { get; set; }

        public virtual Devolution? Devolution { get; set; }
        public virtual Status Status { get; set; } = null!;
    }
}