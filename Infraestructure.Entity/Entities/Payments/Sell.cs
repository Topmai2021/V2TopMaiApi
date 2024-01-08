using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class Sell
    {
        public Sell()
        {
            Devolutions = new HashSet<Devolution>();
            Payments = new HashSet<Payment>();
            ProfileReviews = new HashSet<ProfileReview>();
            SellRequests = new HashSet<SellRequest>();
        }

        public Guid Id { get; set; }
        public Guid? SellerId { get; set; }
        public Guid? BuyerId { get; set; }
        public string? ShippingCode { get; set; }
        public Guid? PublicationId { get; set; }
        public int? Amount { get; set; }
        public float? Total { get; set; }
        public int? TransactionNumber { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime? RealDeliveryDate { get; set; }
        public bool Deleted { get; set; }
        public float? TotalOffered { get; set; }
        public float? TotalWithCommission { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public int StatusId { get; set; }
        public int CurrencyId { get; set; }

        public virtual Currency Currency { get; set; } = null!;
        public virtual Publication? Publication { get; set; }
        public virtual Status Status { get; set; } = null!;
        public virtual ICollection<Devolution> Devolutions { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<ProfileReview> ProfileReviews { get; set; }
        public virtual ICollection<SellRequest> SellRequests { get; set; }

        [NotMapped]
        public List<StatusChange>? StatusChanges { get; set; }

        [NotMapped]
        public SellRequest? SellRequest { get; set; }

        [NotMapped]
        public Devolution? Devolution { get; set; }

        [NotMapped]
        public Profile? Seller { get; set; }

        [NotMapped]
        public List<SellImage>? SellImages { get; set; }

        [NotMapped]
        public Profile? Buyer { get; set; }
    }
}