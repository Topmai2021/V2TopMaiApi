namespace Infraestructure.Entity.Entities.Products
{
    public partial class PublicationShippmentType
    {
        public Guid Id { get; set; }
        public Guid? PublicationId { get; set; }
        public int? ShippmentTypeId { get; set; }
        public float? Price { get; set; }
    }
}