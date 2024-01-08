using Infraestructure.Entity.Entities.Transactions;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Movements = new HashSet<Movement>();
            Payments = new HashSet<Payment>();
            Transactions = new HashSet<Transaction>();
        }

        public string Name { get; set; } = null!;
        public bool Deleted { get; set; }
        public float? Commission { get; set; }
        public int AccreditationDays { get; set; }
        public float? BuyerCommission { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Movement> Movements { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}