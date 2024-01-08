using Infraestructure.Entity.Entities.Payments;

namespace Infraestructure.Entity.Entities.Profiles
{
    public partial class IdentityValidation
    {
        public IdentityValidation()
        {
            IdentityValidationImages = new HashSet<IdentityValidationImage>();
        }

        public Guid Id { get; set; }
        public Guid? ProfileId { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? ResolutionReason { get; set; }
        public int StatusId { get; set; }

        public virtual Profile? Profile { get; set; }
        public virtual Status Status { get; set; } = null!;
        public virtual ICollection<IdentityValidationImage> IdentityValidationImages { get; set; }
    }
}