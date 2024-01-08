using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Products
{
    public partial class FavoriteSeller
    {
        public Guid Id { get; set; }
        public Guid? SellerId { get; set; }
        public Guid? ProfileId { get; set; }
        public DateTime? DateTime { get; set; }
        public bool? Deleted { get; set; }

        public virtual Profile? Seller { get; set; }
    }
}