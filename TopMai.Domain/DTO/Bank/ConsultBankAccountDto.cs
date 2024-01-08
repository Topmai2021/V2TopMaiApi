namespace TopMai.Domain.DTO.Bank
{
    public class ConsultBankAccountDto
    {
        public string AccountHolder { get; set; } = null!;
        public string Alias { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public string Bank { get; set; } = null!;
    }
}
