using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TopMai.Domain.DTO.Transactions.RechargueWallet
{
    public class ConfirmPaymentReference_Dto
    {
        [Required(ErrorMessage ="El campo [Referencia de Pago] es obligatora.")]
        public string PaymentReference { get; set; }
        [Required(ErrorMessage = "El campo [Evidencia (una imagen)] es obligatora.")]
        public IFormFile Evidencia { get; set; }
    }
}
