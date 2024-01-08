using System.ComponentModel.DataAnnotations;
using TopMai.Domain.DTO.CodeValidation;

namespace TopMai.Domain.DTO.User
{
    public class ChangePasswordByEmailDto : ValidationCodeDto
    {
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = null!;
    }
}
