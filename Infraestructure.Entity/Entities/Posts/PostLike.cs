using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Posts
{
    public partial class PostLike
    {
        public Guid Id { get; set; }
        public Guid? PostId { get; set; }
        public Guid? FromId { get; set; }
        public DateTime? DateTime { get; set; }
        public bool? Deleted { get; set; }

        public virtual Profile? From { get; set; }
        public virtual Post? Post { get; set; }
    }
}