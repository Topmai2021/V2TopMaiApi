using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Transactions;
using TopMai.Domain.DTO.Transactions.RechargueWallet;

namespace TopMai.Domain.Services.Transactions.Interfaces
{
    public interface ITransacionServices
    {
        Task NewTransactions(AddTransaction_Dto add, Guid idUser);
        Task<bool> CancelTransaction(Guid idTransaction, Guid idUser);
        List<TransactionDto> GetAllTransactionsByWallet(Guid idWallet, Guid idUser);
        Task TransactionRechargueWallet(RechargueTopMai_Dto add);
        TransactionDto GetByIdTransactionsByWallet(Guid idTransaction);
    }
}
