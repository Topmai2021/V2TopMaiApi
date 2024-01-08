namespace TopMai.Domain.DTO.Transactions
{
    public class AddTransaction_Dto
    {
        public Guid IdWalletOrigen { get; set; }
        public int IdPaymentMethods { get; set; }
        public Guid IdWalletDestination { get; set; }
        public decimal Amount { get; set; }
    }
}
