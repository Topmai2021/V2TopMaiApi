using TopMai.Domain.DTO.Transactions.HistoricalTransactions;

namespace TopMai.Domain.Services.Transactions.Interfaces
{
    public interface IHistoricalTransactionServices
    {
        Task<bool> InsertHistoricalTransaction(AddHistoricalTransaction_Dto historical);

        List<ConsultHistoricalTransactions_Dto> GetAllHistoricalTransacions(Guid IdTransaction);
    }
}
