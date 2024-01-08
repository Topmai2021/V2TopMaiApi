namespace Infraestructure.Entity.Entities.Products
{
    public partial class CartPublication
    {
        public Guid Id { get; set; }
        public Guid? CartId { get; set; }
        public Guid? PublicationId { get; set; }
        public int? Amount { get; set; }
        public bool? Deleted { get; set; }

        public virtual Cart? Cart { get; set; }
        public virtual Publication? Publication { get; set; }
    }
}