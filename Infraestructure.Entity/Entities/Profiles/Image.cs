using Infraestructure.Entity.Entities.Posts;
using Infraestructure.Entity.Entities.Products;

namespace Infraestructure.Entity.Entities.Profiles
{
    public partial class Image
    {
        public Image()
        {
            IdentityValidationImages = new HashSet<IdentityValidationImage>();
            Medals = new HashSet<Medal>();
            PostImages = new HashSet<PostImage>();
            Profiles = new HashSet<Profile>();
            PublicationImages = new HashSet<PublicationImage>();
        }

        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public string? UrlImage { get; set; }
        public Guid? ProfileId { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual ICollection<IdentityValidationImage> IdentityValidationImages { get; set; }
        public virtual ICollection<Medal> Medals { get; set; }
        public virtual ICollection<PostImage> PostImages { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual ICollection<PublicationImage> PublicationImages { get; set; }
    }
}