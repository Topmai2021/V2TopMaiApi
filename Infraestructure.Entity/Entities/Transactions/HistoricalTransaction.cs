using Infraestructure.Entity.Entities.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Entity.Entities.Transactions
{
    public partial class HistoricalTransaction
    {
        public Guid Id { get; set; }
        public Guid IdTransaction { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int IdStatusTransaction { get; set; }
        public decimal Amount { get; set; }
        public string Observation { get; set; } = null!;

        public virtual Status IdStatusTransactionNavigation { get; set; } = null!;
        public virtual Transaction IdTransactionNavigation { get; set; } = null!;
    }
}
