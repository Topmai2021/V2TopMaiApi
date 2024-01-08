namespace Infraestructure.Entity.Entities.Products
{
    public partial class FavoritePublication
    {
        public Guid Id { get; set; }
        public Guid? PublicationId { get; set; }
        public Guid? ProfileId { get; set; }
        public DateTime? DateTime { get; set; }
        public bool? Deleted { get; set; }

        public virtual Publication? Publication { get; set; }
    }
}