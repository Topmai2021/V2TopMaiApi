using Infraestructure.Entity.Entities.Profiles;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Posts
{
    public partial class Post
    {
        public Post()
        {
            PostComments = new HashSet<PostComment>();
            PostImages = new HashSet<PostImage>();
            PostLikes = new HashSet<PostLike>();
        }

        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? Private { get; set; }
        public Guid? PublisherId { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public bool? Promoted { get; set; }
        public DateTime? PublicationDate { get; set; }
        public bool? Deleted { get; set; }

        public virtual Profile? Publisher { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<PostImage> PostImages { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }

        [NotMapped]
        public List<PostComment>? Comments { get; set; }

        [NotMapped]
        public List<PostLike>? Likes { get; set; }

        [NotMapped]
        public int? LikesAmount { get; set; }

        [NotMapped]
        public List<Image>? Images { get; set; }
    }
}