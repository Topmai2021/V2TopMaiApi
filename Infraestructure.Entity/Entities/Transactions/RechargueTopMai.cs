using Infraestructure.Entity.Entities.Payments;

namespace Infraestructure.Entity.Entities.Transactions
{
    public partial class RechargueTopMai
    {
        public Guid Id { get; set; }
        public Guid IdWallet { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Amount { get; set; }
        public Guid IdUser { get; set; }

        public virtual Wallet IdWalletNavigation { get; set; } = null!;
    }
}
