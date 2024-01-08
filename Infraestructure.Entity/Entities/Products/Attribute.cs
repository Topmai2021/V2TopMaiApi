namespace Infraestructure.Entity.Entities.Products
{
    public partial class Attribute
    {
        public Attribute()
        {
            PublicationAttributes = new HashSet<PublicationAttribute>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? CategoryId { get; set; }
        public bool? Deleted { get; set; }
        public bool? Required { get; set; }
        public string? Type { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<PublicationAttribute> PublicationAttributes { get; set; }
    }
}