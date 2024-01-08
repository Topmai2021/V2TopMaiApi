using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Transactions;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class Status
    {
        public Status()
        {
            DevolutionStatusChanges = new HashSet<DevolutionStatusChange>();
            Devolutions = new HashSet<Devolution>();
            HistoricalTransactions = new HashSet<HistoricalTransaction>();
            IdentityValidations = new HashSet<IdentityValidation>();
            Movements = new HashSet<Movement>();
            Payments = new HashSet<Payment>();
            RechargueWallets = new HashSet<RechargueWallet>();
            SellRequests = new HashSet<SellRequest>();
            Sells = new HashSet<Sell>();
            StatusChanges = new HashSet<StatusChange>();
            Transactions = new HashSet<Transaction>();
            TransactionsAccreditations = new HashSet<TransactionsAccreditation>();
            WithdrawalWallets = new HashSet<WithdrawalWallet>();
        }

        public string Name { get; set; } = null!;
        public bool Deleted { get; set; }
        public string Ambit { get; set; } = null!;
        public int Id { get; set; }

        public virtual ICollection<DevolutionStatusChange> DevolutionStatusChanges { get; set; }
        public virtual ICollection<Devolution> Devolutions { get; set; }
        public virtual ICollection<HistoricalTransaction> HistoricalTransactions { get; set; }
        public virtual ICollection<IdentityValidation> IdentityValidations { get; set; }
        public virtual ICollection<Movement> Movements { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<RechargueWallet> RechargueWallets { get; set; }
        public virtual ICollection<SellRequest> SellRequests { get; set; }
        public virtual ICollection<Sell> Sells { get; set; }
        public virtual ICollection<StatusChange> StatusChanges { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<TransactionsAccreditation> TransactionsAccreditations { get; set; }
        public virtual ICollection<WithdrawalWallet> WithdrawalWallets { get; set; }
    }
}