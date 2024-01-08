using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopMai.Domain.DTO.Transactions.RechargueWallet
{
    public class RechargueTopMai_Dto : AddTransaction_Dto
    {
        public Guid IdUser { get; set; }
        public int AmountTopMai { get; set; }
    }
}
