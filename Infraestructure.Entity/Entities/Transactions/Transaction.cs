using Infraestructure.Entity.Entities.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Entity.Entities.Transactions
{
    public partial class Transaction
    {
        public Transaction()
        {
            HistoricalTransactions = new HashSet<HistoricalTransaction>();
            TransactionsAccreditations = new HashSet<TransactionsAccreditation>();
        }

        public Guid Id { get; set; }
        public Guid IdWalletOrigen { get; set; }
        public int IdPaymentMethods { get; set; }
        public Guid IdWalletDestination { get; set; }
        public int IdStatus { get; set; }
        public DateTime TransationDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Observation { get; set; } = null!;

        public virtual PaymentMethod IdPaymentMethodsNavigation { get; set; } = null!;
        public virtual Status IdStatusNavigation { get; set; } = null!;
        public virtual Wallet IdWalletDestinationNavigation { get; set; } = null!;
        public virtual Wallet IdWalletOrigenNavigation { get; set; } = null!;
        public virtual ICollection<HistoricalTransaction> HistoricalTransactions { get; set; }
        public virtual ICollection<TransactionsAccreditation> TransactionsAccreditations { get; set; }
    }
}
