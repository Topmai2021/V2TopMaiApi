using System.ComponentModel.DataAnnotations;

namespace TopMai.Domain.DTO.Transactions.RechargueWallet
{
    public class ReferencePayment_Dto
    {
        [Required]
        public Guid IdWallet { get; set; }
        [Required]
        public int IdTypeOrigenRechargue { get; set; }
        [Required]
        public decimal Amount { get; set; }

    }
}
