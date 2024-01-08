using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopMai.Domain.DTO.Transactions
{
    public class AddTransactionsAccreditation_Dto
    {
        public Guid IdTransaction { get; set; }
        public int Days { get; set; }
    }
}
