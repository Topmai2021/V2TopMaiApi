using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Transactions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class Wallet
    {
        public Wallet()
        {
            Profiles = new HashSet<Profile>();
            RechargueTopMais = new HashSet<RechargueTopMai>();
            RechargueWallets = new HashSet<RechargueWallet>();
            TransactionIdWalletDestinationNavigations = new HashSet<Transaction>();
            TransactionIdWalletOrigenNavigations = new HashSet<Transaction>();
            WithdrawalWallets = new HashSet<WithdrawalWallet>();
        }

        public Guid Id { get; set; }
        public float Money { get; set; }
        public string? Password { get; set; }
        public string? Pin { get; set; }
        public long? WalletNumber { get; set; }
        public int CurrencyId { get; set; }

        public virtual Currency Currency { get; set; } = null!;
        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual ICollection<RechargueTopMai> RechargueTopMais { get; set; }
        public virtual ICollection<RechargueWallet> RechargueWallets { get; set; }
        public virtual ICollection<Transaction> TransactionIdWalletDestinationNavigations { get; set; }
        public virtual ICollection<Transaction> TransactionIdWalletOrigenNavigations { get; set; }
        public virtual ICollection<WithdrawalWallet> WithdrawalWallets { get; set; }

        [NotMapped]
        public List<Movement>? Movements;
    }
}