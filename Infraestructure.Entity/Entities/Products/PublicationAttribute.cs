namespace Infraestructure.Entity.Entities.Products
{
    public partial class PublicationAttribute
    {
        public Guid Id { get; set; }
        public Guid? AttributeId { get; set; }
        public Guid? PublicationId { get; set; }
        public string? Value { get; set; }
        public bool? Deleted { get; set; }

        public virtual Attribute? Attribute { get; set; }
        public virtual Publication? Publication { get; set; }
    }
}