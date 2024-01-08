using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class StorePayRequest
    {
        public Guid Id { get; set; }
        public Guid? ProfileId { get; set; }
        public string? BarCodeUrl { get; set; }
        public string? Reference { get; set; }
        public bool? Deleted { get; set; }
        public float? Amount { get; set; }
        public DateTime? DateTime { get; set; }

        public virtual Profile? Profile { get; set; }
    }
}