using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Products
{
    public partial class PublicationImage
    {
        public Guid Id { get; set; }
        public Guid? ImageId { get; set; }
        public Guid? PublicationId { get; set; }
        public bool? Deleted { get; set; }
        public int? Number { get; set; }

        public virtual Image? Image { get; set; }
        public virtual Publication? Publication { get; set; }
    }
}