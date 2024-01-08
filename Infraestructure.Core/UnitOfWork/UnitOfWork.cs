using Infraestructure.Core.Data;
using Infraestructure.Core.Repository;
using Infraestructure.Core.Repository.Interface;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Entities.Locations;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Posts;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Transactions;
using Infraestructure.Entity.Entities.Users;
using Microsoft.EntityFrameworkCore.Storage;
using Version = Infraestructure.Entity.Entities.Other.Version;

namespace Infraestructure.Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        #region Attributes

        private readonly DataContext _context;
        private bool disposed = false;

        #endregion Attributes

        #region builder

        public UnitOfWork(DataContext context)
        {
            this._context = context;
        }

        #endregion builder

        #region Properties
        private IChatRepository chatRepository;
        private IRepository<ChatType> chatTypeRepository;
        private IMessageRepository messageRepository;
        private IRepository<MessageType> messageTypeRepository;
        private IRepository<RoomOfConversation> roomOfConversationRepository;
        private IRepository<Address> addressRepository;
        private IRepository<City> cityRepository;
        private IRepository<Country> countryRepository;
        private IRepository<Province> provinceRepository;
        private IRepository<Shippment> shippmentRepository;
        private IRepository<CardType> cardTypeRepository;
        private IRepository<Currency> currencyRepository;
        private IRepository<DetailPayment> detailPaymentRepository;
        private IRepository<Devolution> devolutionRepository;
        private IRepository<DevolutionStatusChange> devolutionStatusChangeRepository;

        private IRepository<Status> statusRepository;
        private IRepository<Movement> movementRepository;

        private IRepository<Card> cardRepository;

        private IRepository<IdentityValidation> identityValidationRepository;

        private IRepository<IdentityValidationImage> identityValidationImageRepository;
        private IRepository<Payment> paymentRepository;
        private IRepository<PaymentMethod> paymentMethodRepository;
        private IRepository<StorePayRequest> storePayRequestRepository;

        private IRepository<Sell> sellRepository;
        private IRepository<SellRequest> sellRequestRepository;
        private IRepository<Wallet> walletRepository;
        private IRepository<BankAccount> bankAccountRepository;
        private IRepository<Bank> bankRepository;
        private IRepository<Pin> pinRepository;

        private IRepository<QuestionAnswer> questionAnswerRepository;
        private IRepository<Question> questionRepository;

        private IRepository<Post> postRepository;
        private IRepository<PostComment> postCommentRepository;
        private IRepository<PostImage> postImageRepository;
        private IRepository<PostLike> postLikeRepository;
        private IRepository<Entity.Entities.Products.Attribute> attributeRepository;
        private IRepository<Cart> cartRepository;
        private IRepository<CartPublication> cartPublicationRepository;
        private IRepository<Category> categoryRepository;
        private IRepository<Condition> conditionRepository;
        private IRepository<FavoritePublication> favoritePublicationRepository;
        private IRepository<FavoriteSeller> favoriteSellerRepository;
        private IRepository<Publication> publicationRepository;
        private IRepository<PublicationAttribute> publicationAttributeRepository;
        private IRepository<PublicationComment> publicationCommentRepository;
        private IRepository<PublicationLike> publicationLikeRepository;

        private IRepository<PublicationImage> publicationImageRepository;
        private IRepository<PublicationShippmentType> publicationShippmentTypeRepository;
        private IRepository<ShippmentType> shippmentTypeRepository;
        private IRepository<Subcategory> subcategoryRepository;
        private IRepository<Weight> weightRepository;
        private IRepository<Contact> contactRepository;
        private IRepository<Friend> friendRepository;
        private IRepository<FriendRequest> friendRequestRepository;
        private IRepository<Gender> genderRepository;
        private IRepository<Image> imageRepository;
        private IRepository<Medal> medalRepository;
        private IRepository<Profile> profileRepository;
        private IRepository<ProfileCard> profileCardRepository;
        private IRepository<ProfileReview> profileReviewRepository;
        private IRepository<ProfileUrl> profileUrlRepository;
        private IRepository<ReviewType> reviewTypeRepository;
        private IRepository<Permission> permissionRepository;
        private IRepository<Role> roleRepository;
        private IRepository<User> userRepository;
        private IRepository<VerifyCode> verifyCodeRepository;
        private IRepository<MessageConfiguration> messageConfigurationRepository;
        private IRepository<ChatConfiguration> chatConfigurationRepository;
        private IRepository<Version> versionRepository;
        private IRepository<MovementType> movementTypeRepository;


        private IRepository<HistoricalTransaction> historicalTransactionRepository;
        private IRepository<RechargueWallet> rechargueWalletRepository;
        private IRepository<Transaction> transactiontRepository;
        private IRepository<TransactionsAccreditation> transactionsAccreditationRepository;
        private IRepository<TypeOrigenRechargue> typeOrigenRechargueRepository;
        private IRepository<WithdrawalWallet> withdrawalWalletRepository;
        private IRepository<RechargueTopMai> rechargueTopMaiRepository;

        private IRepository<CodeValidation> codeValidationRepository;

        #endregion


        #region Members

        public IChatRepository ChatRepository
        {
            get
            {
                if (this.chatRepository == null)
                    this.chatRepository = new ChatRepository(_context);

                return chatRepository;
            }
        }
        public IRepository<ChatType> ChatTypeRepository
        {
            get
            {
                if (this.chatTypeRepository == null)
                    this.chatTypeRepository = new Repository<ChatType>(_context);

                return chatTypeRepository;
            }
        }
        public IMessageRepository MessageRepository
        {
            get
            {
                if (this.messageRepository == null)
                    this.messageRepository = new MessageRepository(_context);

                return messageRepository;
            }
        }
        public IRepository<MessageType> MessageTypeRepository
        {
            get
            {
                if (this.messageTypeRepository == null)
                    this.messageTypeRepository = new Repository<MessageType>(_context);

                return messageTypeRepository;
            }
        }
        public IRepository<RoomOfConversation> RoomOfConversationRepository
        {
            get
            {
                if (this.roomOfConversationRepository == null)
                    this.roomOfConversationRepository = new Repository<RoomOfConversation>(_context);

                return roomOfConversationRepository;
            }
        }
        public IRepository<Address> AddressRepository
        {
            get
            {
                if (this.addressRepository == null)
                    this.addressRepository = new Repository<Address>(_context);

                return addressRepository;
            }
        }
        public IRepository<City> CityRepository
        {
            get
            {
                if (this.cityRepository == null)
                    this.cityRepository = new Repository<City>(_context);

                return cityRepository;
            }
        }
        public IRepository<Country> CountryRepository
        {
            get
            {
                if (this.countryRepository == null)
                    this.countryRepository = new Repository<Country>(_context);

                return countryRepository;
            }
        }
        public IRepository<Province> ProvinceRepository
        {
            get
            {
                if (this.provinceRepository == null)
                    this.provinceRepository = new Repository<Province>(_context);

                return provinceRepository;
            }
        }
        public IRepository<Shippment> ShippmentRepository
        {
            get
            {
                if (this.shippmentRepository == null)
                    this.shippmentRepository = new Repository<Shippment>(_context);

                return shippmentRepository;
            }
        }
        public IRepository<CardType> CardTypeRepository
        {
            get
            {
                if (this.cardTypeRepository == null)
                    this.cardTypeRepository = new Repository<CardType>(_context);

                return cardTypeRepository;
            }
        }
        public IRepository<Currency> CurrencyRepository
        {
            get
            {
                if (this.currencyRepository == null)
                    this.currencyRepository = new Repository<Currency>(_context);

                return currencyRepository;
            }
        }
        public IRepository<DetailPayment> DetailPaymentRepository
        {
            get
            {
                if (this.detailPaymentRepository == null)
                    this.detailPaymentRepository = new Repository<DetailPayment>(_context);

                return detailPaymentRepository;
            }
        }
        public IRepository<Devolution> DevolutionRepository
        {
            get
            {
                if (this.devolutionRepository == null)
                    this.devolutionRepository = new Repository<Devolution>(_context);

                return devolutionRepository;
            }
        }
        public IRepository<DevolutionStatusChange> DevolutionStatusChangeRepository
        {
            get
            {
                if (this.devolutionStatusChangeRepository == null)
                    this.devolutionStatusChangeRepository = new Repository<DevolutionStatusChange>(_context);

                return devolutionStatusChangeRepository;
            }
        }

        public IRepository<Status> StatusRepository
        {
            get
            {
                if (this.statusRepository == null)
                    this.statusRepository = new Repository<Status>(_context);

                return statusRepository;
            }
        }

        public IRepository<Movement> MovementRepository
        {
            get
            {
                if (this.movementRepository == null)
                    this.movementRepository = new Repository<Movement>(_context);

                return movementRepository;
            }
        }
        public IRepository<Card> CardRepository
        {
            get
            {
                if (this.cardRepository == null)
                    this.cardRepository = new Repository<Card>(_context);

                return cardRepository;
            }
        }

        public IRepository<IdentityValidation> IdentityValidationRepository
        {
            get
            {
                if (this.identityValidationRepository == null)
                    this.identityValidationRepository = new Repository<IdentityValidation>(_context);

                return identityValidationRepository;
            }
        }

        public IRepository<IdentityValidationImage> IdentityValidationImageRepository
        {
            get
            {
                if (this.identityValidationImageRepository == null)
                    this.identityValidationImageRepository = new Repository<IdentityValidationImage>(_context);

                return identityValidationImageRepository;
            }
        }
        public IRepository<Payment> PaymentRepository
        {
            get
            {
                if (this.paymentRepository == null)
                    this.paymentRepository = new Repository<Payment>(_context);

                return paymentRepository;
            }
        }
        public IRepository<PaymentMethod> PaymentMethodRepository
        {
            get
            {
                if (this.paymentMethodRepository == null)
                    this.paymentMethodRepository = new Repository<PaymentMethod>(_context);

                return paymentMethodRepository;
            }
        }

        public IRepository<StorePayRequest> StorePayRequestRepository
        {
            get
            {
                if (this.storePayRequestRepository == null)
                    this.storePayRequestRepository = new Repository<StorePayRequest>(_context);

                return storePayRequestRepository;
            }
        }
        public IRepository<Sell> SellRepository
        {
            get
            {
                if (this.sellRepository == null)
                    this.sellRepository = new Repository<Sell>(_context);

                return sellRepository;
            }
        }

        public IRepository<SellRequest> SellRequestRepository
        {
            get
            {
                if (this.sellRequestRepository == null)
                    this.sellRequestRepository = new Repository<SellRequest>(_context);

                return sellRequestRepository;
            }
        }
        public IRepository<Wallet> WalletRepository
        {
            get
            {
                if (this.walletRepository == null)
                    this.walletRepository = new Repository<Wallet>(_context);

                return walletRepository;
            }
        }


        public IRepository<BankAccount> BankAccountRepository
        {
            get
            {
                if (this.bankAccountRepository == null)
                    this.bankAccountRepository = new Repository<BankAccount>(_context);

                return bankAccountRepository;
            }
        }

        public IRepository<Bank> BankRepository
        {
            get
            {
                if (this.bankRepository == null)
                    this.bankRepository = new Repository<Bank>(_context);

                return bankRepository;
            }
        }

        public IRepository<Pin> PinRepository
        {
            get
            {
                if (this.pinRepository == null)
                    this.pinRepository = new Repository<Pin>(_context);

                return pinRepository;
            }
        }

        public IRepository<Question> QuestionRepository
        {
            get
            {
                if (this.questionRepository == null)
                    this.questionRepository = new Repository<Question>(_context);

                return questionRepository;
            }
        }
        public IRepository<QuestionAnswer> QuestionAnswerRepository
        {
            get
            {
                if (this.questionAnswerRepository == null)
                    this.questionAnswerRepository = new Repository<QuestionAnswer>(_context);

                return questionAnswerRepository;
            }
        }


        public IRepository<Post> PostRepository
        {
            get
            {
                if (this.postRepository == null)
                    this.postRepository = new Repository<Post>(_context);

                return postRepository;
            }
        }
        public IRepository<PostComment> PostCommentRepository
        {
            get
            {
                if (this.postCommentRepository == null)
                    this.postCommentRepository = new Repository<PostComment>(_context);

                return postCommentRepository;
            }
        }
        public IRepository<PostImage> PostImageRepository
        {
            get
            {
                if (this.postImageRepository == null)
                    this.postImageRepository = new Repository<PostImage>(_context);

                return postImageRepository;
            }
        }
        public IRepository<PostLike> PostLikeRepository
        {
            get
            {
                if (this.postLikeRepository == null)
                    this.postLikeRepository = new Repository<PostLike>(_context);

                return postLikeRepository;
            }
        }
        public IRepository<Entity.Entities.Products.Attribute> AttributeRepository
        {
            get
            {
                if (this.attributeRepository == null)
                    this.attributeRepository = new Repository<Entity.Entities.Products.Attribute>(_context);

                return attributeRepository;
            }
        }
        public IRepository<Cart> CartRepository
        {
            get
            {
                if (this.cartRepository == null)
                    this.cartRepository = new Repository<Cart>(_context);

                return cartRepository;
            }
        }
        public IRepository<CartPublication> CartPublicationRepository
        {
            get
            {
                if (this.cartPublicationRepository == null)
                    this.cartPublicationRepository = new Repository<CartPublication>(_context);

                return cartPublicationRepository;
            }
        }
        public IRepository<Category> CategoryRepository
        {
            get
            {
                if (this.categoryRepository == null)
                    this.categoryRepository = new Repository<Category>(_context);

                return categoryRepository;
            }
        }
        public IRepository<Condition> ConditionRepository
        {
            get
            {
                if (this.conditionRepository == null)
                    this.conditionRepository = new Repository<Condition>(_context);

                return conditionRepository;
            }
        }
        public IRepository<FavoritePublication> FavoritePublicationRepository
        {
            get
            {
                if (this.favoritePublicationRepository == null)
                    this.favoritePublicationRepository = new Repository<FavoritePublication>(_context);

                return favoritePublicationRepository;
            }
        }
        public IRepository<FavoriteSeller> FavoriteSellerRepository
        {
            get
            {
                if (this.favoriteSellerRepository == null)
                    this.favoriteSellerRepository = new Repository<FavoriteSeller>(_context);

                return favoriteSellerRepository;
            }
        }
        public IRepository<Publication> PublicationRepository
        {
            get
            {
                if (this.publicationRepository == null)
                    this.publicationRepository = new Repository<Publication>(_context);

                return publicationRepository;
            }
        }
        public IRepository<PublicationAttribute> PublicationAttributeRepository
        {
            get
            {
                if (this.publicationAttributeRepository == null)
                    this.publicationAttributeRepository = new Repository<PublicationAttribute>(_context);

                return publicationAttributeRepository;
            }
        }
        public IRepository<PublicationComment> PublicationCommentRepository
        {
            get
            {
                if (this.publicationCommentRepository == null)
                    this.publicationCommentRepository = new Repository<PublicationComment>(_context);

                return publicationCommentRepository;
            }
        }
        public IRepository<PublicationImage> PublicationImageRepository
        {
            get
            {
                if (this.publicationImageRepository == null)
                    this.publicationImageRepository = new Repository<PublicationImage>(_context);

                return publicationImageRepository;
            }
        }
        public IRepository<PublicationShippmentType> PublicationShippmentTypeRepository
        {
            get
            {
                if (this.publicationShippmentTypeRepository == null)
                    this.publicationShippmentTypeRepository = new Repository<PublicationShippmentType>(_context);

                return publicationShippmentTypeRepository;
            }
        }
        public IRepository<ShippmentType> ShippmentTypeRepository
        {
            get
            {
                if (this.shippmentTypeRepository == null)
                    this.shippmentTypeRepository = new Repository<ShippmentType>(_context);

                return shippmentTypeRepository;
            }
        }
        public IRepository<Subcategory> SubcategoryRepository
        {
            get
            {
                if (this.subcategoryRepository == null)
                    this.subcategoryRepository = new Repository<Subcategory>(_context);

                return subcategoryRepository;
            }
        }
        public IRepository<Weight> WeightRepository
        {
            get
            {
                if (this.weightRepository == null)
                    this.weightRepository = new Repository<Weight>(_context);

                return weightRepository;
            }
        }
        public IRepository<Contact> ContactRepository
        {
            get
            {
                if (this.contactRepository == null)
                    this.contactRepository = new Repository<Contact>(_context);

                return contactRepository;
            }
        }
        public IRepository<Friend> FriendRepository
        {
            get
            {
                if (this.friendRepository == null)
                    this.friendRepository = new Repository<Friend>(_context);

                return friendRepository;
            }
        }
        public IRepository<FriendRequest> FriendRequestRepository
        {
            get
            {
                if (this.friendRequestRepository == null)
                    this.friendRequestRepository = new Repository<FriendRequest>(_context);

                return friendRequestRepository;
            }
        }
        public IRepository<Gender> GenderRepository
        {
            get
            {
                if (this.genderRepository == null)
                    this.genderRepository = new Repository<Gender>(_context);

                return genderRepository;
            }
        }
        public IRepository<Image> ImageRepository
        {
            get
            {
                if (this.imageRepository == null)
                    this.imageRepository = new Repository<Image>(_context);

                return imageRepository;
            }
        }
        public IRepository<Medal> MedalRepository
        {
            get
            {
                if (this.medalRepository == null)
                    this.medalRepository = new Repository<Medal>(_context);

                return medalRepository;
            }
        }
        public IRepository<Profile> ProfileRepository
        {
            get
            {
                if (this.profileRepository == null)
                    this.profileRepository = new Repository<Profile>(_context);

                return profileRepository;
            }
        }
        public IRepository<ProfileCard> ProfileCardRepository
        {
            get
            {
                if (this.profileCardRepository == null)
                    this.profileCardRepository = new Repository<ProfileCard>(_context);

                return profileCardRepository;
            }
        }
        public IRepository<ProfileReview> ProfileReviewRepository
        {
            get
            {
                if (this.profileReviewRepository == null)
                    this.profileReviewRepository = new Repository<ProfileReview>(_context);

                return profileReviewRepository;
            }
        }
        public IRepository<ProfileUrl> ProfileUrlRepository
        {
            get
            {
                if (this.profileUrlRepository == null)
                    this.profileUrlRepository = new Repository<ProfileUrl>(_context);

                return profileUrlRepository;
            }
        }
        public IRepository<ReviewType> ReviewTypeRepository
        {
            get
            {
                if (this.reviewTypeRepository == null)
                    this.reviewTypeRepository = new Repository<ReviewType>(_context);

                return reviewTypeRepository;
            }
        }
        public IRepository<Permission> PermissionRepository
        {
            get
            {
                if (this.permissionRepository == null)
                    this.permissionRepository = new Repository<Permission>(_context);

                return permissionRepository;
            }
        }
        public IRepository<Role> RoleRepository
        {
            get
            {
                if (this.roleRepository == null)
                    this.roleRepository = new Repository<Role>(_context);

                return roleRepository;
            }
        }
        public IRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                    this.userRepository = new Repository<User>(_context);

                return userRepository;
            }
        }
        public IRepository<VerifyCode> VerifyCodeRepository
        {
            get
            {
                if (this.verifyCodeRepository == null)
                    this.verifyCodeRepository = new Repository<VerifyCode>(_context);

                return verifyCodeRepository;
            }
        }
        public IRepository<MessageConfiguration> MessageConfigurationRepository
        {
            get
            {
                if (this.messageConfigurationRepository == null)
                    this.messageConfigurationRepository = new Repository<MessageConfiguration>(_context);

                return messageConfigurationRepository;
            }
        }

        public IRepository<ChatConfiguration> ChatConfigurationRepository
        {
            get
            {
                if (this.chatConfigurationRepository == null)
                    this.chatConfigurationRepository = new Repository<ChatConfiguration>(_context);

                return chatConfigurationRepository;
            }
        }

        public IRepository<Version> VersionRepository
        {
            get
            {
                if (this.versionRepository == null)
                    this.versionRepository = new Repository<Version>(_context);

                return versionRepository;
            }
        }



        public IRepository<PublicationLike> PublicationLikeRepository
        {
            get
            {
                if (this.publicationLikeRepository == null)
                    this.publicationLikeRepository = new Repository<PublicationLike>(_context);

                return publicationLikeRepository;
            }
        }

        public IRepository<MovementType> MovementTypeRepository
        {
            get
            {
                if (this.movementTypeRepository == null)
                    this.movementTypeRepository = new Repository<MovementType>(_context);

                return movementTypeRepository;
            }
        }



        public IRepository<HistoricalTransaction> HistoricalTransactionRepository
        {
            get
            {
                if (this.historicalTransactionRepository == null)
                    this.historicalTransactionRepository = new Repository<HistoricalTransaction>(_context);

                return historicalTransactionRepository;
            }
        }

        public IRepository<RechargueWallet> RechargueWalletRepository
        {
            get
            {
                if (this.rechargueWalletRepository == null)
                    this.rechargueWalletRepository = new Repository<RechargueWallet>(_context);

                return rechargueWalletRepository;
            }
        }
        public IRepository<Transaction> TransactiontRepository
        {
            get
            {
                if (this.transactiontRepository == null)
                    this.transactiontRepository = new Repository<Transaction>(_context);

                return transactiontRepository;
            }
        }

        public IRepository<TransactionsAccreditation> TransactionsAccreditationRepository
        {
            get
            {
                if (this.transactionsAccreditationRepository == null)
                    this.transactionsAccreditationRepository = new Repository<TransactionsAccreditation>(_context);

                return transactionsAccreditationRepository;
            }
        }

        public IRepository<TypeOrigenRechargue> TypeOrigenRechargueRepository
        {
            get
            {
                if (this.typeOrigenRechargueRepository == null)
                    this.typeOrigenRechargueRepository = new Repository<TypeOrigenRechargue>(_context);

                return typeOrigenRechargueRepository;
            }
        }

        public IRepository<WithdrawalWallet> WithdrawalWalletRepository
        {
            get
            {
                if (this.withdrawalWalletRepository == null)
                    this.withdrawalWalletRepository = new Repository<WithdrawalWallet>(_context);

                return withdrawalWalletRepository;
            }
        }


        public IRepository<RechargueTopMai> RechargueTopMaiRepository
        {
            get
            {
                if (this.rechargueTopMaiRepository == null)
                    this.rechargueTopMaiRepository = new Repository<RechargueTopMai>(_context);

                return rechargueTopMaiRepository;
            }
        }

        public IRepository<CodeValidation> CodeValidationRepository
        {
            get
            {
                if (this.codeValidationRepository == null)
                    this.codeValidationRepository = new Repository<CodeValidation>(_context);

                return codeValidationRepository;
            }
        }


        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        #endregion


        protected virtual void Dispose(bool disposing)
        {

            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> Save() => await _context.SaveChangesAsync();
        public int SaveChanges() => _context.SaveChanges();
    }
}
