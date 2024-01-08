using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infraestructure.Entity.Request
{
    public class PublicationUpdateRequest
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? UrlPrincipalImage { get; set; }
        public float? Price { get; set; }
        public int CurrencyId { get; set; }
        public Guid PublisherId { get; set; }
        public bool? HasPersonalDelivery { get; set; }
        public bool? HasFreeShippment { get; set; }
        public bool? HasPickup { get; set; }
        public Guid? SubcategoryId { get; set; }
        public string? Weight { get; set; }
        public float? ShippmentPrice { get; set; }
        public string? Condition { get; set; }
        public bool? HasInternationalShipping { get; set; }
        public bool? ReceiveOffers { get; set; }
        public bool Deleted { get; set; }
    }
}
