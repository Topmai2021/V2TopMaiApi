using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Products
{
    public partial class Cart
    {
        public Cart()
        {
            CartPublications = new List<CartPublication>();
        }

        public Guid Id { get; set; }
        public Guid? ProfileId { get; set; }
        public DateTime? ModificationDateTime { get; set; }
        public float? Total { get; set; }
        public bool? Deleted { get; set; }

        public virtual Profile? Profile { get; set; }
        public virtual List<CartPublication> CartPublications { get; set; }
    }
}