using TopMai.Domain.DTO.Transactions.HistoricalTransactions;

namespace TopMai.Domain.DTO.Transactions
{
    public class TransactionDto
    {
        public TransactionDto()
        {
            HistoricalTransactions = new List<ConsultHistoricalTransactions_Dto>();
        }

        public Guid IdTransation { get; set; }
        public int IdTypeTransaction { get; set; }
        public string Status { get; set; }
        public DateTime TransationDate { get; set; }
        public string StrPaymentMethods { get; set; }
        public string UserNameProfileOrigen { get; set; }
        public string UrlImagenProfileOrigen { get; set; }
        public string UserNameProfileDestionation { get; set; }
        public string UrlImagenProfileDestionation { get; set; }
        public decimal Amount { get; set; }

        public List<ConsultHistoricalTransactions_Dto> HistoricalTransactions { get; set; }
    }
}
