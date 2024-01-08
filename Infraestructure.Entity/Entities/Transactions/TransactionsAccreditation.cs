using Infraestructure.Entity.Entities.Payments;

namespace Infraestructure.Entity.Entities.Transactions
{
    public partial class TransactionsAccreditation
    {
        public Guid Id { get; set; }
        public Guid IdTransaction { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime AccreditationDate { get; set; }
        public int IdStatus { get; set; }

        public virtual Status IdStatusNavigation { get; set; } = null!;
        public virtual Transaction IdTransactionNavigation { get; set; } = null!;
    }
}
