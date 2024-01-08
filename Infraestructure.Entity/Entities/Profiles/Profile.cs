using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Entities.Locations;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Posts;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Profiles
{
    public partial class Profile
    {
        public Profile()
        {
            Addresses = new HashSet<Address>();
            Cards = new HashSet<Card>();
            Carts = new HashSet<Cart>();
            ChatConfigurations = new HashSet<ChatConfiguration>();
            ChatIdProfileReceiverNavigations = new HashSet<Chat>();
            ChatIdProfileSenderNavigations = new HashSet<Chat>();
            Contacts = new HashSet<Contact>();
            FavoriteSellers = new HashSet<FavoriteSeller>();
            IdentityValidations = new HashSet<IdentityValidation>();
            MessageConfigurations = new HashSet<MessageConfiguration>();
            Messages = new HashSet<Message>();
            MovementAuthorizedBies = new HashSet<Movement>();
            MovementProfiles = new HashSet<Movement>();
            PaymentFroms = new HashSet<Payment>();
            PaymentTos = new HashSet<Payment>();
            PostComments = new HashSet<PostComment>();
            PostLikes = new HashSet<PostLike>();
            Posts = new HashSet<Post>();
            ProfileCards = new HashSet<ProfileCard>();
            ProfileReviews = new HashSet<ProfileReview>();
            ProfileUrls = new HashSet<ProfileUrl>();
            PublicationComments = new HashSet<PublicationComment>();
            PublicationLikes = new HashSet<PublicationLike>();
            StorePayRequests = new HashSet<StorePayRequest>();
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public string? ProfileUrl { get; set; }
        public DateTime? BirthDate { get; set; }
        public Guid? CountryId { get; set; }
        public int? Sales { get; set; }
        public string? LastName { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public bool? Verified { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Description { get; set; }
        public string? Lenguages { get; set; }
        public string? UrlPrincipalImage { get; set; }
        public Guid? PrincipalImageId { get; set; }
        public float? Valoration { get; set; }
        public Guid? ImageId { get; set; }
        public string? ConnectionId { get; set; }
        public string? OneSignalConnectionId { get; set; }
        public bool Deleted { get; set; }
        public string? Land { get; set; }
        public DateTime? MarketplaceReactivationDate { get; set; }
        public int? MarketplaceReactivationCount { get; set; }
        public Guid? WalletId { get; set; }
        public bool? IdentityValidated { get; set; }
        public string? OpenPayCustomerId { get; set; }
        public int GenderId { get; set; }

        public virtual Country? Country { get; set; }
        public virtual Gender Gender { get; set; } = null!;
        public virtual Image? Image { get; set; }
        public virtual Wallet? Wallet { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<ChatConfiguration> ChatConfigurations { get; set; }
        public virtual ICollection<Chat> ChatIdProfileReceiverNavigations { get; set; }
        public virtual ICollection<Chat> ChatIdProfileSenderNavigations { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<FavoriteSeller> FavoriteSellers { get; set; }
        public virtual ICollection<IdentityValidation> IdentityValidations { get; set; }
        public virtual ICollection<MessageConfiguration> MessageConfigurations { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Movement> MovementAuthorizedBies { get; set; }
        public virtual ICollection<Movement> MovementProfiles { get; set; }
        public virtual ICollection<Payment> PaymentFroms { get; set; }
        public virtual ICollection<Payment> PaymentTos { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<ProfileCard> ProfileCards { get; set; }
        public virtual ICollection<ProfileReview> ProfileReviews { get; set; }
        public virtual ICollection<ProfileUrl> ProfileUrls { get; set; }
        public virtual ICollection<PublicationComment> PublicationComments { get; set; }
        public virtual ICollection<PublicationLike> PublicationLikes { get; set; }
        public virtual ICollection<StorePayRequest> StorePayRequests { get; set; }
        public virtual ICollection<User> Users { get; set; }


        [NotMapped]
        public string FullName { get { return $"{this.Name} {this.LastName}"; } }
    }
}