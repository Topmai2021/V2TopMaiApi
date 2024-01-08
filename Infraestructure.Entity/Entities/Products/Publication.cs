using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Entities.Complaints;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Profiles;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Products
{
    public partial class Publication
    {
        public Publication()
        {
            CartPublications = new HashSet<CartPublication>();
            Chats = new HashSet<Chat>();
            Complaints = new HashSet<Complaint>();
            DetailPayments = new HashSet<DetailPayment>();
            FavoritePublications = new HashSet<FavoritePublication>();
            PublicationAttributes = new HashSet<PublicationAttribute>();
            PublicationComments = new HashSet<PublicationComment>();
            PublicationImagesImages = new HashSet<PublicationImage>();
            PublicationLikes = new HashSet<PublicationLike>();
            Sells = new HashSet<Sell>();

            PublicationImages = new List<Image>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public float? Price { get; set; }
        public float? PriceNote { get; set; }
        public bool Deleted { get; set; }
        public string? PublicationType { get; set; }
        public Guid? PublisherId { get; set; }
        public DateTime? PublicationDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? HasPersonalDelivery { get; set; }
        public bool? HasFreeShippment { get; set; }
        public bool? HasPickup { get; set; }
        public int? Sales { get; set; }
        public int? Stock { get; set; }
        public int? Status { get; set; }
        public string? UrlPrincipalImage { get; set; }
        public Guid? SubcategoryId { get; set; }
        public float? Valoration { get; set; }
        public int? Visits { get; set; }
        public int? CommentsCount { get; set; }
        public string? Weight { get; set; }
        public float? ShippmentPrice { get; set; }
        public string? Condition { get; set; }
        public bool? Promoted { get; set; }
        public bool? HasInternationalShipping { get; set; }
        public bool? ReceiveOffers { get; set; }
        public long? Number { get; set; }
        public int CurrencyId { get; set; }
        public virtual Profile Publisher { get; set; }

        public virtual Currency Currency { get; set; } = null!;
        public virtual Subcategory? Subcategory { get; set; }
        public virtual ICollection<CartPublication> CartPublications { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
        public virtual ICollection<Complaint> Complaints { get; set; }
        public virtual ICollection<DetailPayment> DetailPayments { get; set; }
        public virtual ICollection<FavoritePublication> FavoritePublications { get; set; }
        public virtual ICollection<PublicationAttribute> PublicationAttributes { get; set; }
        public virtual ICollection<PublicationComment> PublicationComments { get; set; }
        public virtual ICollection<PublicationImage> PublicationImagesImages { get; set; }
        public virtual ICollection<PublicationLike> PublicationLikes { get; set; }
        public virtual ICollection<Sell> Sells { get; set; }

        [NotMapped]
        public Category? Category { get; set; }
        [NotMapped]
        public List<Image>? PublicationImages { get; set; }
        [NotMapped]
        public long TotalCount { get; set; }
        [NotMapped]
        public long TotalPage { get; set; }
        [NotMapped]
        public long PageNumber { get; set; }
    }
}