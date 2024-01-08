namespace Infraestructure.Entity.Entities.Products
{
    public partial class Weight
    {
        public Guid Id { get; set; }
        public string? Range { get; set; }
        public bool? Deleted { get; set; }
    }
}