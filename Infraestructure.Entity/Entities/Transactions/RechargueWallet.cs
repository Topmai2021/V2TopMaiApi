using Infraestructure.Entity.Entities.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Entity.Entities.Transactions
{
    public partial class RechargueWallet
    {
        public Guid Id { get; set; }
        public Guid IdWallet { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int IdStatus { get; set; }
        public int IdTypeOrigenRechargue { get; set; }
        public string PaymentReference { get; set; } = null!;
        
        public decimal Amount { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid? ImagenId { get; set; }

        public Guid IdUserOrigen { get; set; }
        public Guid IdUserApprover { get; set; }
        public string Observation { get; set; } = null!;
        public virtual Status IdStatusNavigation { get; set; } = null!;
        public virtual TypeOrigenRechargue IdTypeOrigenRechargueNavigation { get; set; } = null!;
        public virtual Wallet IdWalletNavigation { get; set; } = null!;
    }
}
