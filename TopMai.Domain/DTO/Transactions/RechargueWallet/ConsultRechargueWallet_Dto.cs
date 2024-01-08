using Infraestructure.Entity.Entities.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.DTO.Profiles;

namespace TopMai.Domain.DTO.Transactions.RechargueWallet
{
    public class ConsultRechargueWallet_Dto
    {
        public Guid IdRechargue { get; set; }
        public string Status { get; set; } = null!;
        public string TypeOrigenRechargue { get; set; } = null!;
        public string PaymentReference { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Observation { get; set; }
        public string UrlImage { get; set; }
    }
    public class ConsultAllRechargueWallet_Dto
    {
        public Guid IdRechargue { get; set; }
        public string Status { get; set; } = null!;
        public string TypeOrigenRechargue { get; set; } = null!;
        public string PaymentReference { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Observation { get; set; }
        public string UrlImage { get; set; }
        public Profile? Profiles { get; set; }
    }
}
