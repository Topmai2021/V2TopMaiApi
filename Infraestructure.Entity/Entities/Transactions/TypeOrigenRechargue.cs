using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Entity.Entities.Transactions
{
    public partial class TypeOrigenRechargue
    {
        public TypeOrigenRechargue()
        {
            RechargueWallets = new HashSet<RechargueWallet>();
        }

        public int Id { get; set; }
        public string TypeOrigen { get; set; } = null!;

        public virtual ICollection<RechargueWallet> RechargueWallets { get; set; }
    }
}
