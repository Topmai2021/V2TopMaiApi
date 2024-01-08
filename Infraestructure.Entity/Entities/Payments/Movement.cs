using Infraestructure.Entity.Entities.Profiles;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class Movement
    {
        public Guid Id { get; set; }
        public float Amount { get; set; }
        public bool Deleted { get; set; }
        public string? ConektaOrderId { get; set; }
        public string? Detail { get; set; }
        public Guid? ProfileId { get; set; }
        public string? UrlImage { get; set; }
        public Guid? AuthorizedById { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public string? ResolutionReason { get; set; }
        public int StatusId { get; set; }
        public int? PaymentMethodId { get; set; }
        public int MovementTypeId { get; set; }
        public int CurrencyId { get; set; }

        public virtual Profile? AuthorizedBy { get; set; }
        public virtual Currency Currency { get; set; } = null!;
        public virtual MovementType MovementType { get; set; } = null!;
        public virtual PaymentMethod? PaymentMethod { get; set; }
        public virtual Profile? Profile { get; set; }
        public virtual Status Status { get; set; } = null!;
        [NotMapped]
        public long TotalCount { get; set; }
        [NotMapped]
        public long TotalPage { get; set; }
        [NotMapped]
        public long PageNumber { get; set; }
    }
}