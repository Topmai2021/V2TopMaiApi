using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Products
{
    public partial class PublicationLike
    {
        public Guid Id { get; set; }
        public Guid? PublicationId { get; set; }
        public Guid? FromId { get; set; }
        public DateTime? DateTime { get; set; }
        public bool? Deleted { get; set; }

        public virtual Profile? From { get; set; }
        public virtual Publication? Publication { get; set; }
    }
}