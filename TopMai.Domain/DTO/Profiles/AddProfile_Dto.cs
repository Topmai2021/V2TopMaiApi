using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopMai.Domain.DTO.Profiles
{
    public class AddProfile_Dto
    {
        [Required(ErrorMessage = "El campo [Id] es requerido")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "El campo [BirthDate] es requerido")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "El campo [GenderId] es requerido")]
        public int GenderId { get; set; }

        [Required(ErrorMessage = "El campo [Apellido] es requerido")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El campo [Nombre] es requerido")]
        public string Name { get; set; }


        public string? ProfileUrl { get; set; }

        public Guid? CountryId { get; set; }


        public string? Phone { get; set; }
        public bool? Verified { get; set; } = false;
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Description { get; set; }
        public string? Lenguages { get; set; }
        public string? UrlPrincipalImage { get; set; }

        public Guid? ImageId { get; set; }

        public string? Land { get; set; }
    }
}
