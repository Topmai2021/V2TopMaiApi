using Infraestructure.Core.Repository;
using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Entities.Locations;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Posts;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Users;
using Infraestructure.Entity.Entities.Other;
using Version = Infraestructure.Entity.Entities.Other.Version;
using Infraestructure.Entity.Entities.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Infraestructure.Core.Repository.Interface;

namespace Infraestructure.Core.UnitOfWork.Interface
{
    public interface IUnitOfWork
    {
        IChatRepository ChatRepository { get; }
        IRepository<ChatType> ChatTypeRepository { get; }
        IMessageRepository MessageRepository { get; }
        IRepository<MessageType> MessageTypeRepository { get; }
        IRepository<RoomOfConversation> RoomOfConversationRepository { get; }
        IRepository<Address> AddressRepository { get; }
        IRepository<City> CityRepository { get; }
        IRepository<Country> CountryRepository { get; }
        IRepository<Province> ProvinceRepository { get; }
        IRepository<Shippment> ShippmentRepository { get; }
        IRepository<CardType> CardTypeRepository { get; }
        IRepository<Currency> CurrencyRepository { get; }
        IRepository<DetailPayment> DetailPaymentRepository { get; }
        IRepository<Devolution> DevolutionRepository { get; }
        IRepository<DevolutionStatusChange> DevolutionStatusChangeRepository { get; }
        IRepository<Status> StatusRepository { get; }

        IRepository<Movement> MovementRepository { get; }
        IRepository<Card> CardRepository { get; }

        IRepository<IdentityValidation> IdentityValidationRepository { get; }
        IRepository<IdentityValidationImage> IdentityValidationImageRepository { get; }

        IRepository<Payment> PaymentRepository { get; }
        IRepository<PaymentMethod> PaymentMethodRepository { get; }
        IRepository<StorePayRequest> StorePayRequestRepository { get; }

        IRepository<Sell> SellRepository { get; }
        IRepository<SellRequest> SellRequestRepository { get; }


        IRepository<Wallet> WalletRepository { get; }
        IRepository<BankAccount> BankAccountRepository { get; }
        IRepository<Bank> BankRepository { get; }
        IRepository<Pin> PinRepository { get; }

        IRepository<Question> QuestionRepository { get; }
        IRepository<QuestionAnswer> QuestionAnswerRepository { get; }


        IRepository<Post> PostRepository { get; }
        IRepository<PostComment> PostCommentRepository { get; }
        IRepository<PostImage> PostImageRepository { get; }
        IRepository<PostLike> PostLikeRepository { get; }

        IRepository<Entity.Entities.Products.Attribute> AttributeRepository { get; }
        IRepository<Cart> CartRepository { get; }
        IRepository<CartPublication> CartPublicationRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<Condition> ConditionRepository { get; }
        IRepository<FavoritePublication> FavoritePublicationRepository { get; }
        IRepository<FavoriteSeller> FavoriteSellerRepository { get; }
        IRepository<Publication> PublicationRepository { get; }
        IRepository<PublicationAttribute> PublicationAttributeRepository { get; }
        IRepository<PublicationComment> PublicationCommentRepository { get; }
        IRepository<PublicationLike> PublicationLikeRepository { get; }

        IRepository<PublicationImage> PublicationImageRepository { get; }
        IRepository<PublicationShippmentType> PublicationShippmentTypeRepository { get; }
        IRepository<ShippmentType> ShippmentTypeRepository { get; }
        IRepository<Subcategory> SubcategoryRepository { get; }
        IRepository<Weight> WeightRepository { get; }

        IRepository<Contact> ContactRepository { get; }
        IRepository<Friend> FriendRepository { get; }
        IRepository<FriendRequest> FriendRequestRepository { get; }
        IRepository<Gender> GenderRepository { get; }
        IRepository<Image> ImageRepository { get; }
        IRepository<Medal> MedalRepository { get; }
        IRepository<Profile> ProfileRepository { get; }
        IRepository<ProfileCard> ProfileCardRepository { get; }
        IRepository<ProfileReview> ProfileReviewRepository { get; }
        IRepository<ProfileUrl> ProfileUrlRepository { get; }
        IRepository<ReviewType> ReviewTypeRepository { get; }

        IRepository<Permission> PermissionRepository { get; }
        IRepository<Role> RoleRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<VerifyCode> VerifyCodeRepository { get; }
        IRepository<MessageConfiguration> MessageConfigurationRepository { get; }
        IRepository<ChatConfiguration> ChatConfigurationRepository { get; }
        IRepository<Version> VersionRepository { get; }
        IRepository<MovementType> MovementTypeRepository { get; }


        IRepository<HistoricalTransaction> HistoricalTransactionRepository { get; }
        IRepository<RechargueWallet> RechargueWalletRepository { get; }
        IRepository<Transaction> TransactiontRepository { get; }
        IRepository<TransactionsAccreditation> TransactionsAccreditationRepository { get; }
        IRepository<TypeOrigenRechargue> TypeOrigenRechargueRepository { get; }
        IRepository<WithdrawalWallet> WithdrawalWalletRepository { get; }
        IRepository<RechargueTopMai> RechargueTopMaiRepository { get; }
        IRepository<CodeValidation> CodeValidationRepository { get; }

        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();

        new void Dispose();

        Task<int> Save();
        int SaveChanges();
    }

}
