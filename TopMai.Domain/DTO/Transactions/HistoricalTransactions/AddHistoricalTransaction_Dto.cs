namespace TopMai.Domain.DTO.Transactions.HistoricalTransactions
{
    public class AddHistoricalTransaction_Dto
    {
        public Guid IdTransaction { get; set; }
        public int IdStatusTransaction { get; set; }
        public string Observation { get; set; }
        public decimal Amount { get; set; }
    }
}
