using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class Payment
    {
        public Payment()
        {
            DetailPayments = new HashSet<DetailPayment>();
        }

        public Guid Id { get; set; }
        public DateTime? DateHour { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public Guid? FromId { get; set; }
        public Guid? ToId { get; set; }
        public bool? Deleted { get; set; }
        public Guid? SellId { get; set; }
        public float? Total { get; set; }
        public DateTime? FrozenReceiptDate { get; set; }
        public Guid? PaymentTypeId { get; set; }
        public float? TotalWithoutCommission { get; set; }
        public int StatusId { get; set; }
        public int PaymentMethodId { get; set; }
        public int CurrencyId { get; set; }

        public virtual Currency Currency { get; set; } = null!;
        public virtual Profile? From { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; } = null!;
        public virtual Sell? Sell { get; set; }
        public virtual Status Status { get; set; } = null!;
        public virtual Profile? To { get; set; }
        public virtual ICollection<DetailPayment> DetailPayments { get; set; }
    }
}