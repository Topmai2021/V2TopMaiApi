using Infraestructure.Entity.Entities.Locations;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infraestructure.Entity.DTOs
{
    public class SellRequestDTO
    {
        [JsonIgnore]
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PublicationId { get; set; }
        public int? Amount { get; set; }
        public float? Total { get; set; }
        public string? MeetingPlace { get; set; }
        public string? MeetingTime { get; set; }
        public string? ClothingColor { get; set; }
        public bool? WithShippment { get; set; }
        public Guid? AddressId { get; set; }
        public int CurrencyId { get; set; }
        public int StatusId { get; set; }
    }
}
