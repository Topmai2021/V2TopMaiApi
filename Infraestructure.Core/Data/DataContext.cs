using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Entities.Complaints;
using Infraestructure.Entity.Entities.Locations;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Posts;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Transactions;
using Infraestructure.Entity.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Core.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Entity.Entities.Products.Attribute> Attributes { get; set; } = null!;
        public virtual DbSet<Bank> Banks { get; set; } = null!;
        public virtual DbSet<BankAccount> BankAccounts { get; set; } = null!;
        public virtual DbSet<Card> Cards { get; set; } = null!;
        public virtual DbSet<CardType> CardTypes { get; set; } = null!;
        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<CartPublication> CartPublications { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<ChatConfiguration> ChatConfigurations { get; set; } = null!;
        public virtual DbSet<ChatType> ChatTypes { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<CodeValidation> CodeValidations { get; set; } = null!;
        public virtual DbSet<Complaint> Complaints { get; set; } = null!;
        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Currency> Currencys { get; set; } = null!;
        public virtual DbSet<DetailPayment> DetailPayments { get; set; } = null!;
        public virtual DbSet<Devolution> Devolutions { get; set; } = null!;
        public virtual DbSet<DevolutionStatusChange> DevolutionStatusChanges { get; set; } = null!;
        public virtual DbSet<FavoritePublication> FavoritePublications { get; set; } = null!;
        public virtual DbSet<FavoriteSeller> FavoriteSellers { get; set; } = null!;
        public virtual DbSet<Friend> Friends { get; set; } = null!;
        public virtual DbSet<FriendRequest> FriendRequests { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<HistoricalTransaction> HistoricalTransactions { get; set; } = null!;
        public virtual DbSet<IdentityValidation> IdentityValidations { get; set; } = null!;
        public virtual DbSet<IdentityValidationImage> IdentityValidationImages { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Medal> Medals { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<MessageConfiguration> MessageConfigurations { get; set; } = null!;
        public virtual DbSet<MessageType> MessageTypes { get; set; } = null!;
        public virtual DbSet<Movement> Movements { get; set; } = null!;
        public virtual DbSet<MovementType> MovementTypes { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public virtual DbSet<Pin> Pins { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PostComment> PostComments { get; set; } = null!;
        public virtual DbSet<PostImage> PostImages { get; set; } = null!;
        public virtual DbSet<PostLike> PostLikes { get; set; } = null!;
        public virtual DbSet<Profile> Profiles { get; set; } = null!;
        public virtual DbSet<ProfileCard> ProfileCards { get; set; } = null!;
        public virtual DbSet<ProfileReview> ProfileReviews { get; set; } = null!;
        public virtual DbSet<ProfileUrl> ProfileUrls { get; set; } = null!;
        public virtual DbSet<Province> Provinces { get; set; } = null!;
        public virtual DbSet<Publication> Publications { get; set; } = null!;
        public virtual DbSet<PublicationAttribute> PublicationAttributes { get; set; } = null!;
        public virtual DbSet<PublicationComment> PublicationComments { get; set; } = null!;
        public virtual DbSet<PublicationImage> PublicationImages { get; set; } = null!;
        public virtual DbSet<PublicationLike> PublicationLikes { get; set; } = null!;
        public virtual DbSet<PublicationShippmentType> PublicationShippmentTypes { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; } = null!;
        public virtual DbSet<Reason> Reasons { get; set; } = null!;
        public virtual DbSet<RechargueTopMai> RechargueTopMais { get; set; } = null!;
        public virtual DbSet<RechargueWallet> RechargueWallets { get; set; } = null!;
        public virtual DbSet<ReviewType> ReviewTypes { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RoomOfConversation> RoomOfConversations { get; set; } = null!;
        public virtual DbSet<Sell> Sells { get; set; } = null!;
        public virtual DbSet<SellRequest> SellRequests { get; set; } = null!;
        public virtual DbSet<Shippment> Shippments { get; set; } = null!;
        public virtual DbSet<ShippmentType> ShippmentTypes { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<StatusChange> StatusChanges { get; set; } = null!;
        public virtual DbSet<StorePayRequest> StorePayRequests { get; set; } = null!;
        public virtual DbSet<Subcategory> Subcategories { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<TransactionsAccreditation> TransactionsAccreditations { get; set; } = null!;
        public virtual DbSet<TypeOrigenRechargue> TypeOrigenRechargues { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Entity.Entities.Other.Version> Versions { get; set; } = null!;
        public virtual DbSet<Wallet> Wallets { get; set; } = null!;
        public virtual DbSet<Weight> Weights { get; set; } = null!;
        public virtual DbSet<WithdrawalWallet> WithdrawalWallets { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.ProfileId);
            });

            modelBuilder.Entity<Entity.Entities.Products.Attribute>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Attributes)
                    .HasForeignKey(d => d.CategoryId);
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Cbu).HasColumnName("CBU");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.BankAccounts)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK__BankAccou__BankI__503BEA1C");
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OpenPayCardId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityCode)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("Cards_FK");
            });

            modelBuilder.Entity<CardType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProfileId);
            });

            modelBuilder.Entity<CartPublication>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartPublications)
                    .HasForeignKey(d => d.CartId);

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.CartPublications)
                    .HasForeignKey(d => d.PublicationId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasIndex(e => new { e.IdProfileSender, e.IdProfileReceiver, e.ChatTypeId, e.PublicationId }, "UQ__Chats__4A978272B7F560B8");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.ChatType)
                    .WithMany(p => p.Chats)
                    .HasForeignKey(d => d.ChatTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.IdProfileReceiverNavigation)
                    .WithMany(p => p.ChatIdProfileReceiverNavigations)
                    .HasForeignKey(d => d.IdProfileReceiver)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chats_Profiles1");

                entity.HasOne(d => d.IdProfileSenderNavigation)
                    .WithMany(p => p.ChatIdProfileSenderNavigations)
                    .HasForeignKey(d => d.IdProfileSender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chats_Profiles");

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.Chats)
                    .HasForeignKey(d => d.PublicationId)
                    .HasConstraintName("FK__Chats__Publicati__56E8E7AB");
            });

            modelBuilder.Entity<ChatConfiguration>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.ChatConfigurations)
                    .HasForeignKey(d => d.ChatId);

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ChatConfigurations)
                    .HasForeignKey(d => d.ProfileId);
            });

            modelBuilder.Entity<ChatType>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.ProvinceId);
            });

            modelBuilder.Entity<CodeValidation>(entity =>
            {
                entity.ToTable("CodeValidation");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.CodeValidations)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CodeValidation_Users");
            });

            modelBuilder.Entity<Complaint>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.Complaints)
                    .HasForeignKey(d => d.PublicationId);

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.Complaints)
                    .HasForeignKey(d => d.ReasonId);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasIndex(e => new { e.ProfileId, e.ContactProfileId }, "Contacts_UN");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.ContactProfile)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.ContactProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Contacts__Contac__5BAD9CC8");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Abbreviation).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<DetailPayment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.DetailPayments)
                    .HasForeignKey(d => d.PaymentId);

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.DetailPayments)
                    .HasForeignKey(d => d.PublicationId);
            });

            modelBuilder.Entity<Devolution>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.UpdateDate).HasPrecision(0);

                entity.HasOne(d => d.Sell)
                    .WithMany(p => p.Devolutions)
                    .HasForeignKey(d => d.SellId);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Devolutions)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Devolutio__Statu__3FD07829");
            });

            modelBuilder.Entity<DevolutionStatusChange>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EndDate).HasPrecision(0);

                entity.Property(e => e.StartDate).HasPrecision(0);

                entity.HasOne(d => d.Devolution)
                    .WithMany(p => p.DevolutionStatusChanges)
                    .HasForeignKey(d => d.DevolutionId)
                    .HasConstraintName("FK__Devolutio__Devol__607251E5");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.DevolutionStatusChanges)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Devolutio__Statu__3CF40B7E");
            });

            modelBuilder.Entity<FavoritePublication>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.FavoritePublications)
                    .HasForeignKey(d => d.PublicationId);
            });

            modelBuilder.Entity<FavoriteSeller>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.FavoriteSellers)
                    .HasForeignKey(d => d.SellerId);
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<FriendRequest>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<HistoricalTransaction>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Observation).HasMaxLength(300);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdStatusTransactionNavigation)
                    .WithMany(p => p.HistoricalTransactions)
                    .HasForeignKey(d => d.IdStatusTransaction)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoricalTransactions_Statuses");

                entity.HasOne(d => d.IdTransactionNavigation)
                    .WithMany(p => p.HistoricalTransactions)
                    .HasForeignKey(d => d.IdTransaction)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoricalTransactions_Transaction");
            });

            modelBuilder.Entity<IdentityValidation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreationDate).HasPrecision(0);

                entity.Property(e => e.ResolutionReason)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasPrecision(0);

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.IdentityValidations)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("IdentityValidation_FK_1");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.IdentityValidations)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IdentityValidation_FK");
            });

            modelBuilder.Entity<IdentityValidationImage>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdentityValidation)
                    .WithMany(p => p.IdentityValidationImages)
                    .HasForeignKey(d => d.IdentityValidationId)
                    .HasConstraintName("IdentityValidationImage_FK");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.IdentityValidationImages)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("IdentityValidationImage_FK_1");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Medal>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Medals)
                    .HasForeignKey(d => d.ImageId);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ChatId);

                entity.HasOne(d => d.From)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.FromId);

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<MessageConfiguration>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.MessageConfigurations)
                    .HasForeignKey(d => d.MessageId);

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.MessageConfigurations)
                    .HasForeignKey(d => d.ProfileId);
            });

            modelBuilder.Entity<MessageType>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<Movement>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.AuthorizedBy)
                    .WithMany(p => p.MovementAuthorizedBies)
                    .HasForeignKey(d => d.AuthorizedById);

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MovementType)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(d => d.MovementTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(d => d.PaymentMethodId);

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.MovementProfiles)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK__Movements__Profi__6FB49575");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<MovementType>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FrozenReceiptDate).HasPrecision(0);

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.From)
                    .WithMany(p => p.PaymentFroms)
                    .HasForeignKey(d => d.FromId)
                    .HasConstraintName("FK__Payments__FromId__756D6ECB");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payments__Paymen__4959E263");

                entity.HasOne(d => d.Sell)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.SellId);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.To)
                    .WithMany(p => p.PaymentTos)
                    .HasForeignKey(d => d.ToId)
                    .HasConstraintName("FK__Payments__ToId__7755B73D");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<Pin>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.PublisherId);
            });

            modelBuilder.Entity<PostComment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.From)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.FromId);

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.PostId);
            });

            modelBuilder.Entity<PostImage>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.PostImages)
                    .HasForeignKey(d => d.ImageId);

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostImages)
                    .HasForeignKey(d => d.PostId);
            });

            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.From)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.FromId);

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.PostId);
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IdentityValidated).HasDefaultValueSql("((0))");

                entity.Property(e => e.MarketplaceReactivationDate).HasPrecision(0);

                entity.Property(e => e.OpenPayCustomerId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Profiles)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Profiles)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Profiles)
                    .HasForeignKey(d => d.ImageId);

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.Profiles)
                    .HasForeignKey(d => d.WalletId)
                    .HasConstraintName("FK__Profiles__Wallet__0697FACD");
            });

            modelBuilder.Entity<ProfileCard>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.CardType)
                    .WithMany(p => p.ProfileCards)
                    .HasForeignKey(d => d.CardTypeId);

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProfileCards)
                    .HasForeignKey(d => d.ProfileId);
            });

            modelBuilder.Entity<ProfileReview>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.ReviewType)
                    .WithMany(p => p.ProfileReviews)
                    .HasForeignKey(d => d.ReviewTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Sell)
                    .WithMany(p => p.ProfileReviews)
                    .HasForeignKey(d => d.SellId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProfileRe__SellI__03BB8E22");

                entity.HasOne(d => d.To)
                    .WithMany(p => p.ProfileReviews)
                    .HasForeignKey(d => d.ToId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ProfileUrl>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProfileUrls)
                    .HasForeignKey(d => d.ProfileId);
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Publication>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.HasInternationalShipping).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Publications)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Subcategory)
                    .WithMany(p => p.Publications)
                    .HasForeignKey(d => d.SubcategoryId);
            });

            modelBuilder.Entity<PublicationAttribute>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Attribute)
                    .WithMany(p => p.PublicationAttributes)
                    .HasForeignKey(d => d.AttributeId);

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.PublicationAttributes)
                    .HasForeignKey(d => d.PublicationId);
            });

            modelBuilder.Entity<PublicationComment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Comment).HasMaxLength(2000);

                entity.HasOne(d => d.From)
                    .WithMany(p => p.PublicationComments)
                    .HasForeignKey(d => d.FromId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.PublicationComments)
                    .HasForeignKey(d => d.PublicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PublicationImage>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.PublicationImages)
                    .HasForeignKey(d => d.ImageId);

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.PublicationImagesImages)
                    .HasForeignKey(d => d.PublicationId);
            });

            modelBuilder.Entity<PublicationLike>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.From)
                    .WithMany(p => p.PublicationLikes)
                    .HasForeignKey(d => d.FromId);

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.PublicationLikes)
                    .HasForeignKey(d => d.PublicationId);
            });

            modelBuilder.Entity<PublicationShippmentType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<QuestionAnswer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Answer)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reason>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<RechargueTopMai>(entity =>
            {
                entity.ToTable("RechargueTopMai");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdWalletNavigation)
                    .WithMany(p => p.RechargueTopMais)
                    .HasForeignKey(d => d.IdWallet)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RechargueTopMai_Wallets");
            });

            modelBuilder.Entity<RechargueWallet>(entity =>
            {
                entity.ToTable("RechargueWallet");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Observation).HasMaxLength(1000);

                entity.Property(e => e.PaymentReference).HasMaxLength(20);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.RechargueWallets)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RechargueWallet_Statuses");

                entity.HasOne(d => d.IdTypeOrigenRechargueNavigation)
                    .WithMany(p => p.RechargueWallets)
                    .HasForeignKey(d => d.IdTypeOrigenRechargue)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RechargueWallet_TypeOrigenRechargue");

                entity.HasOne(d => d.IdWalletNavigation)
                    .WithMany(p => p.RechargueWallets)
                    .HasForeignKey(d => d.IdWallet)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RechargueWallet_Wallets");
            });

            modelBuilder.Entity<ReviewType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<RoomOfConversation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sell>(entity =>
            {
                entity.HasIndex(e => new { e.Total, e.SellerId, e.BuyerId, e.PublicationId, e.DateTime, e.RealDeliveryDate }, "Sells_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EstimatedDeliveryDate).HasPrecision(0);

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Sells)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.Sells)
                    .HasForeignKey(d => d.PublicationId);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Sells)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SellRequest>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ClothingColor)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DateTime).HasPrecision(0);

                entity.Property(e => e.DeliveryType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EndDateTime).HasPrecision(0);

                entity.Property(e => e.MeetingPlace)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MeetingTime)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.SellRequests)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK__SellReque__Addre__18B6AB08");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.SellRequests)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SellReque__Curre__2F9A1060");

                entity.HasOne(d => d.Sell)
                    .WithMany(p => p.SellRequests)
                    .HasForeignKey(d => d.SellId)
                    .HasConstraintName("FK__SellReque__SellI__1A9EF37A");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.SellRequests)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SellReque__Statu__308E3499");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SellRequests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__SellReque__UserI__1C873BEC");
            });

            modelBuilder.Entity<Shippment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Personalized)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ShippmentCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Ambit).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<StatusChange>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EndDate).HasPrecision(0);

                entity.Property(e => e.StartDate).HasPrecision(0);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.StatusChanges)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StatusCha__Statu__56B3DD81");
            });

            modelBuilder.Entity<StorePayRequest>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BarCodeUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DateTime).HasPrecision(0);

                entity.Property(e => e.Reference)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.StorePayRequests)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("StorePayRequest_FK");
            });

            modelBuilder.Entity<Subcategory>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Subcategories)
                    .HasForeignKey(d => d.CategoryId);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Observation).HasMaxLength(300);

                entity.Property(e => e.TransationDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdPaymentMethodsNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.IdPaymentMethods)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_PaymentMethods");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Statuses");

                entity.HasOne(d => d.IdWalletDestinationNavigation)
                    .WithMany(p => p.TransactionIdWalletDestinationNavigations)
                    .HasForeignKey(d => d.IdWalletDestination)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Wallets1");

                entity.HasOne(d => d.IdWalletOrigenNavigation)
                    .WithMany(p => p.TransactionIdWalletOrigenNavigations)
                    .HasForeignKey(d => d.IdWalletOrigen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Wallets");
            });

            modelBuilder.Entity<TransactionsAccreditation>(entity =>
            {
                entity.ToTable("TransactionsAccreditation");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AccreditationDate).HasColumnType("datetime");

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.TransactionsAccreditations)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionAccreditation_Statuses");

                entity.HasOne(d => d.IdTransactionNavigation)
                    .WithMany(p => p.TransactionsAccreditations)
                    .HasForeignKey(d => d.IdTransaction)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionAccreditation_Transaction");
            });

            modelBuilder.Entity<TypeOrigenRechargue>(entity =>
            {
                entity.ToTable("TypeOrigenRechargue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.TypeOrigen).HasMaxLength(200);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ProfileId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Entity.Entities.Other.Version>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Weight>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<WithdrawalWallet>(entity =>
            {
                entity.ToTable("WithdrawalWallet");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ApplicationDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdBankAccountNavigation)
                    .WithMany(p => p.WithdrawalWallets)
                    .HasForeignKey(d => d.IdBankAccount)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WithdrawalWallet_BankAccounts");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.WithdrawalWallets)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WithdrawalWallet_Statuses");

                entity.HasOne(d => d.IdWalletNavigation)
                    .WithMany(p => p.WithdrawalWallets)
                    .HasForeignKey(d => d.IdWallet)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WithdrawalWallet_Wallets");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }

}
