using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopMai.Domain.DTO.Products
{
    public class ConsultPublication_Dto
    {
        public Guid Id { get; set; }
        public float? Price { get; set; }
        public string StrCurrency { get; set; } = null!;
        public string? UrlPrincipalImage { get; set; } 

    }
}
