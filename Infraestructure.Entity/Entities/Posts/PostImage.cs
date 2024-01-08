using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Posts
{
    public partial class PostImage
    {
        public Guid Id { get; set; }
        public Guid? ImageId { get; set; }
        public Guid? PostId { get; set; }
        public bool? Deleted { get; set; }

        public virtual Image? Image { get; set; }
        public virtual Post? Post { get; set; }
    }
}