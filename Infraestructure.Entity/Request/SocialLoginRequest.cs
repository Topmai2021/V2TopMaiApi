using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Entity.Request
{
    public class SocialLoginRequest
    {
        [Required(ErrorMessage = "El email es requerido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El social user id es requerido")]
        public string? SocialUserId { get; set; }
    }
}
