using Infraestructure.Entity.Entities.Payments;

namespace Infraestructure.Entity.Entities.Transactions
{
    public partial class WithdrawalWallet
    {
        public Guid Id { get; set; }
        public Guid IdWallet { get; set; }
        public int IdStatus { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid IdBankAccount { get; set; }

        public virtual BankAccount IdBankAccountNavigation { get; set; } = null!;
        public virtual Status IdStatusNavigation { get; set; } = null!;
        public virtual Wallet IdWalletNavigation { get; set; } = null!;
    }
}
