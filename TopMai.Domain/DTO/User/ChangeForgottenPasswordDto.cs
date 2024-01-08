using System;
using System.ComponentModel.DataAnnotations;
using Common.Utils.Enums;

namespace TopMai.Domain.DTO.User
{
	public class ChangeForgottenPasswordDto
	{
        [Required(ErrorMessage = "El User es obligatorio")]
        public string? UserLogin { get; set; }

        [Required(ErrorMessage = "El Código es obligatorio")]
        public int Code { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "User login type is mandatory")]
        public Enums.TypeCodeValidation Type { get; set; }
    }
}

