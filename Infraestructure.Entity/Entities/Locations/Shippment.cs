namespace Infraestructure.Entity.Entities.Locations
{
    public partial class Shippment
    {
        public Guid Id { get; set; }
        public Guid? AddressFromId { get; set; }
        public Guid? AddressToId { get; set; }
        public string? ProviderName { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public DateTime? ShippmentDateTime { get; set; }
        public int? Status { get; set; }
        public bool? Deleted { get; set; }
        public float? Width { get; set; }
        public float? Height { get; set; }
        public float? Length { get; set; }
        public float? Weight { get; set; }
        public float? Amount { get; set; }
        public string? Personalized { get; set; }
        public string? ShippmentCode { get; set; }
        public float? Total { get; set; }
        public Guid? ProfileId { get; set; }
    }
}