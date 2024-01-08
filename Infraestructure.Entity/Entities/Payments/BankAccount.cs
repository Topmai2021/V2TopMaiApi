using Infraestructure.Entity.Entities.Transactions;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class BankAccount
    {
        public BankAccount()
        {
            WithdrawalWallets = new HashSet<WithdrawalWallet>();
        }

        public Guid Id { get; set; }
        public string? AccountHolder { get; set; }
        public string? Alias { get; set; }
        public string? AccountNumber { get; set; }
        public string? Cbu { get; set; }
        public bool? Active { get; set; }
        public Guid? UserId { get; set; }
        public Guid? BankId { get; set; }
        public bool? IsAdmin { get; set; }

        public virtual Bank? Bank { get; set; }
        public virtual ICollection<WithdrawalWallet> WithdrawalWallets { get; set; }
    }
}