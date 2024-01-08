using System.ComponentModel.DataAnnotations;
using Common.Utils.Enums;

namespace TopMai.Domain.DTO.CodeValidation
{
    public class ValidationCodeDto
    {
        [Required(ErrorMessage = "El User es obligatorio")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "El Código es obligatorio")]
        public int Code { get; set; }
    }
}
