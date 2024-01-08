using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.DTO.Transactions;

namespace TopMai.Domain.Services.Transactions.Interfaces
{
    public interface ITransactionsAccreditationServices
    {
        Task<bool> NewTransactionAcreditation(AddTransactionsAccreditation_Dto add);
    }
}
