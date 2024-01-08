using Infraestructure.Entity.Entities.Locations;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Users;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class SellRequest
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PublicationId { get; set; }
        public int? Amount { get; set; }
        public float? Total { get; set; }
        public DateTime? DateTime { get; set; }
        public Guid? SellId { get; set; }
        public bool Deleted { get; set; }
        public string? MeetingPlace { get; set; }
        public string? MeetingTime { get; set; }
        public string? ClothingColor { get; set; }
        public float? TotalWithCommission { get; set; }
        public float? TotalOffered { get; set; }
        public bool? WithShippment { get; set; }
        public string? DeliveryType { get; set; }
        public DateTime? EndDateTime { get; set; }
        public Guid? AddressId { get; set; }
        public int StatusId { get; set; }
        public int CurrencyId { get; set; }


        public virtual Address? Address { get; set; }
        public virtual Currency Currency { get; set; } = null!;
        public virtual Sell? Sell { get; set; }
        public virtual Status Status { get; set; } = null!;
        public virtual User? User { get; set; }
        public virtual Publication? Publication { get; set; }
    }
}