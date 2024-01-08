using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Entity.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string accessData { get; set; }

        [Required(ErrorMessage = "La contraseña es requerido")]
        public string? password { get; set; }
    }
}