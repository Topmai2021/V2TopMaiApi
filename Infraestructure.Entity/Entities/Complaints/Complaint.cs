using Infraestructure.Entity.Entities.Products;

namespace Infraestructure.Entity.Entities.Complaints
{
    public partial class Complaint
    {
        public Guid Id { get; set; }
        public Guid? PublicationId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ReasonId { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? Deleted { get; set; }
        public Guid? UserToReportId { get; set; }

        public virtual Publication? Publication { get; set; }
        public virtual Reason? Reason { get; set; }
    }
}