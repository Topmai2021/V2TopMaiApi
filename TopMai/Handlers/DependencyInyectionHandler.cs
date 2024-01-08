using Infraestructure.Core.Repository;
using Infraestructure.Core.UnitOfWork;
using Infraestructure.Core.UnitOfWork.Interface;
using TopMai.Domain.Services.Chats;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Domain.Services.Complaints;
using TopMai.Domain.Services.Complaints.Interfaces;
using TopMai.Domain.Services.Emails;
using TopMai.Domain.Services.Emails.Interfaces;
using TopMai.Domain.Services.Locations;
using TopMai.Domain.Services.Locations.Interfaces;
using TopMai.Domain.Services.Other;
using TopMai.Domain.Services.Other.Interfaces;
using TopMai.Domain.Services.Payments;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Domain.Services.Post;
using TopMai.Domain.Services.Post.Interfaces;
using TopMai.Domain.Services.Products;
using TopMai.Domain.Services.Products.Interfaces;
using TopMai.Domain.Services.Profiles;
using TopMai.Domain.Services.Profiles.Interfaces;
using TopMai.Domain.Services.Transactions;
using TopMai.Domain.Services.Transactions.Interfaces;
using TopMai.Domain.Services.Users;
using TopMai.Domain.Services.Users.Interfaces;

namespace TopMai.Handlers
{
    public static class DependencyInyectionHandler
    {
        public static void DependencyInyectionConfig(IServiceCollection services)
        {
            services.AddScoped<CustomValidationFilterAttribute>();
            // Repository await UnitofWork parameter ctor explicit
            services.AddScoped<UnitOfWork, UnitOfWork>();

            // Infrastructure
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            //services.AddTransient<SeedDb>();

            //**Domain**

            //Profiles
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IGenderService, GenderService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IProfileReviewService, ProfileReviewService>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IIdentityValidationService, IdentityValidationService>();


            //chats
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IChatTypeService, ChatTypeService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IMessageTypeService, MessageTypeService>();
            services.AddTransient<IRoomOfConversationService, RoomOfConversationService>();
            services.AddTransient<IChatConfigurationService, ChatConfigurationService>();
            //complaints
            services.AddTransient<IComplaintService, ComplaintService>();
            services.AddTransient<IReasonService, ReasonService>();

            //emails
            services.AddTransient<IEmailService, EmailService>();
            //locations
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IShippmentService, ShippmentService>();
            //payments
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<IDetailPaymentService, DetailPaymentService>();
            services.AddTransient<ICardService, CardService>();

            services.AddTransient<IPaymentMethodService, PaymentMethodService>();
            services.AddTransient<ISellService, SellService>();
            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<IBankAccountService, BankAccountService>();
            services.AddTransient<IBankService, BankService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IQuestionAnswerService, QuestionAnswerService>();
            services.AddTransient<IPinService, PinService>();



            services.AddTransient<IMovementTypeService, MovementTypeService>();
            services.AddTransient<IMovementService, MovementService>();
            services.AddTransient<IPaymentTypeService, PaymentTypeService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IStorePayRequestService, StorePayRequestService>();

            services.AddTransient<ISellRequestService, SellRequestService>();
            services.AddTransient<IStatusService, StatusService>();
            services.AddTransient<IDevolutionService, DevolutionService>();

            services.AddTransient<IDevolutionStatusChangeService, DevolutionStatusChangeService>();

            //post
            services.AddTransient<IPostCommentService, PostCommentService>();
            services.AddTransient<IPostLikeService, PostLikeService>();
            services.AddTransient<IPostService, PostService>();
            //Products
            services.AddTransient<IAttributeService, AttributeService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IFavoritePublicationService, FavoritePublicationService>();
            services.AddTransient<IFavoriteSellerService, FavoriteSellerService>();
            services.AddTransient<IPublicationAttributeService, PublicationAttributeService>();
            services.AddTransient<IPublicationCommentService, PublicationCommentService>();
            services.AddTransient<IPublicationLikeService, PublicationLikeService>();

            services.AddTransient<IPublicationService, PublicationService>();
            services.AddTransient<IShippmentTypeService, ShippmentTypeService>();
            services.AddTransient<ISubcategoryService, SubcategoryService>();
            services.AddTransient<IWeightService, WeightService>();


            services.AddTransient<IReviewTypeService, ReviewTypeService>();
            //user
            services.AddTransient<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IVersionService, VersionService>();
            services.AddScoped<ICodeValidationServices, CodeValidationServices>();

            //transactions
            services.AddTransient<ITransacionServices, TransacionServices>();
            services.AddTransient<IHistoricalTransactionServices, HistoricalTransactionServices>();
            services.AddTransient<ITransactionsAccreditationServices, TransactionsAccreditationServices>();
            services.AddTransient<IRechargueWalletServices, RechargueWalletServices>();
            services.AddTransient<ITypeOrigenRechargueServices, TypeOrigenRechargueServices>();

        }
    }
}
