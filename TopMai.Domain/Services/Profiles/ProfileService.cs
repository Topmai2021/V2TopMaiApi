using Common.Utils.Enums;
using Common.Utils.Exceptions;
using Common.Utils.Helpers;
using Common.Utils.Resources;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Entities.Locations;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Xml.Linq;
using TopMai.Domain.DTO.Profiles;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Domain.Services.Profiles.Interfaces;

namespace TopMai.Domain.Services.Profiles
{
    public class ProfileService : IProfileService
    {
        private DataContext DBContext;
        private IWalletService _walletService;
        private ICurrencyService _currencyService;
        private readonly IGenderService _genderService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IImageService _imageService;

		#region Builder
		public ProfileService(DataContext dBContext,
							  IWalletService walletService,
							  ICurrencyService currencyService,
							  IUnitOfWork unitOfWork,
							  IConfiguration configuration,
							  IImageService imageService,
							  IGenderService genderService)
		{
			DBContext = dBContext;
			_walletService = walletService;
			_currencyService = currencyService;
			_unitOfWork = unitOfWork;
			_config = configuration;
			_imageService = imageService;
			_genderService = genderService;
		}
		#endregion

		#region Methods

		public List<Profile> GetAll() => DBContext.Profiles.OrderByDescending(x => x.Id).ToList();

        public Profile GetProfile(string lastName, string name)
        {
            return _unitOfWork.ProfileRepository.FirstOrDefault(x => x.LastName.ToLower() == lastName.ToLower()
                                                                             && x.Name.ToLower() == name.ToLower());
        }
        public Profile GetProfile(Guid id) => _unitOfWork.ProfileRepository.FirstOrDefault(x => x.Id == id);

        public async Task<ConsultProfileDto> Get(Guid idProfile)
        {
            Profile profile = _unitOfWork.ProfileRepository.FirstOrDefault(x => x.Id == idProfile, i => i.Image);
            if (profile == null)
                throw new BusinessException(GeneralMessages.ItemNoFound);

            ConsultProfileDto profileDto = new ConsultProfileDto()
            {
                Id = profile.Id,
                City = profile.City,
                Description = profile.Description,
                Land = profile.Land,
                Name = profile.Name,
                LastName = profile.LastName,
                Latitude = profile.Latitude,
                Lenguages = profile.Lenguages,
                Longitude = profile.Longitude,
                Phone = profile.Phone,
                PostalCode = profile.PostalCode,
                ProfileUrl = profile.ProfileUrl,
                Sales = profile.Sales,
                State = profile.State,
                WalletId = profile.WalletId,
                UrlImage = _imageService.GetUrlImage(profile?.Image),
                StrGender = Helper.GetGender(profile.GenderId),
                birthDate = profile.BirthDate,
                createdAt = DBContext.Users.FirstOrDefault(x => x.ProfileId == idProfile)?.RegisterDate ?? DateTime.MinValue,
        };

            if (profileDto.WalletId == null)
            {
                Wallet wallet = new Wallet()
                {
                    Id = Guid.NewGuid(),
                    CurrencyId = (int)Enums.Currency.MXN,
                };

                await _walletService.Post(wallet);
                profile.WalletId = wallet.Id;
                profile.ProfileUrl = $"{profile.Name}{profile.LastName}";
                await Put(profile);
            }

            return profileDto;
        }

        public async Task<Profile> Post(AddProfile_Dto profile)
        {
            Helper.ValidateName(profile.Name);
            Helper.ValidateName(profile.LastName);

            if (profile.BirthDate.ToString().Length < 5)
                throw new BusinessException("Ingrese una fecha de nacimiento válida");

            if ((profile.Name.ToUpper().Contains("TOPMAI") || profile.LastName.ToUpper().Contains("TOPMAI")))
                throw new BusinessException("El nombre o apellido no puede ser Topmai");

            Gender gender = _unitOfWork.GenderRepository.FirstOrDefault(g => g.Id == profile.GenderId);
            if (gender == null)
                throw new BusinessException("El id de genero ingresado no coincide con ningun genero en el sistema ");


            Profile repeatedProfile = _unitOfWork.ProfileRepository.FirstOrDefault(p => p.Id == profile.Id);
            if (repeatedProfile != null)
                throw new BusinessException("Ya se encuentra un perfil asociado a este usuario");


            Profile newProfile = new Profile()
            {
                Id = profile.Id,
                ProfileUrl = profile.ProfileUrl,
                BirthDate = profile.BirthDate,
                Name = profile.Name,
                CountryId = profile.CountryId,
                GenderId = profile.GenderId,
                LastName = profile.LastName,
                Phone = profile.Phone,
                Verified = profile.Verified,
                Latitude = profile.Latitude,
                Longitude = profile.Longitude,
                City = profile.City,
                State = profile.State,
                PostalCode = profile.PostalCode,
                Description = profile.Description,
                Lenguages = profile.Lenguages,
                UrlPrincipalImage = profile.UrlPrincipalImage,
                ImageId = profile.ImageId,
                Land = profile.Land,
                Deleted = false,
                Wallet = new Wallet()
                {
                    Id = Guid.NewGuid(),
                    Money = 0,
                    CurrencyId = (int)Enums.Currency.MXN
                }
            };

            _unitOfWork.ProfileRepository.Insert(newProfile);
            await _unitOfWork.Save();

            return newProfile;

        }
        public async Task<bool> Put(Profile newProfile)
        {
            var profile = GetProfile(newProfile.Id);

            ArgumentNullException.ThrowIfNull(profile, GeneralMessages.ItemNoFound);

            var gender = _genderService.GetByName(newProfile.Gender.Name);

            profile.ProfileUrl = newProfile.ProfileUrl ?? profile.ProfileUrl;
            profile.BirthDate = newProfile.BirthDate ?? profile.BirthDate;
            profile.CountryId = newProfile.CountryId ?? profile.CountryId;
            profile.Sales = newProfile.Sales ?? profile.Sales;
            profile.LastName = newProfile.LastName ?? profile.LastName;
            profile.Name = newProfile.Name ?? profile.Name;
            profile.Phone = newProfile.Phone ?? profile.Phone;
            profile.Verified = newProfile.Verified ?? profile.Verified;
            profile.Latitude = newProfile.Latitude ?? profile.Latitude;
            profile.Longitude = newProfile.Longitude ?? profile.Longitude;
            profile.City = newProfile.City ?? profile.City;
            profile.State = newProfile.State ?? profile.State;
            profile.PostalCode = newProfile.PostalCode ?? profile.PostalCode;
            profile.Description = newProfile.Description ?? profile.Description;
            profile.Lenguages = newProfile.Lenguages ?? profile.Lenguages;
            profile.UrlPrincipalImage = newProfile.UrlPrincipalImage ?? profile.UrlPrincipalImage;
            profile.PrincipalImageId = newProfile.PrincipalImageId ?? profile.PrincipalImageId;
            profile.Valoration = newProfile.Valoration ?? profile.Valoration;
            profile.MarketplaceReactivationDate = newProfile.MarketplaceReactivationDate ?? profile.MarketplaceReactivationDate;
            profile.MarketplaceReactivationCount = newProfile.MarketplaceReactivationCount ?? profile.MarketplaceReactivationCount;
            profile.WalletId = newProfile.WalletId ?? profile.WalletId;
            profile.Gender = gender;
            profile.GenderId = gender.Id;
            profile.Description = newProfile.Description ?? profile.Description;
            profile.Land = newProfile.Land ?? profile.Land;

            _unitOfWork.ProfileRepository.Update(profile);

            return await _unitOfWork.Save() > 0;
        }

        public object SendFriendRequest(Guid fromId, Guid toId)
        {
            var profiles = DBContext.Profiles.Where(u => u.Id == fromId || u.Id == toId).ToList();
            if (profiles == null || profiles.Count != 2)
                return new { error = "Ingrese perfiles válidos" };

            var friendRequest = DBContext.FriendRequests.Where(f => f.FromId == fromId && f.ToId == toId).FirstOrDefault();
            if (friendRequest != null && friendRequest.Deleted == false)
                return new { error = "Ya existe una solicitud de amistad" };

            FriendRequest newFriendRequest = new FriendRequest();
            newFriendRequest.Id = Guid.NewGuid();
            newFriendRequest.Deleted = false;
            newFriendRequest.FromId = fromId;
            newFriendRequest.ToId = toId;
            newFriendRequest.DateTime = DateTime.Now;
            DBContext.FriendRequests.Add(newFriendRequest);
            DBContext.SaveChanges();

            return newFriendRequest;
        }

        public object GetAllFriendRequest(Guid id)
        {
            Profile profile = DBContext.Profiles.Where(p => p.Id == id).FirstOrDefault();
            if (profile == null)
                return new { error = "No se encuentra ningun perfil con la id ingresada" };
            List<FriendRequest> friendRequests = DBContext.FriendRequests.Where(f => f.ToId == id).ToList();

            return friendRequests;
        }

        public object GetAllFriendsToInvite(Guid id)
        {
            Profile profile = DBContext.Profiles.Where(p => p.Id == id).FirstOrDefault();
            List<FriendToInvite> friendsToInvite = new List<FriendToInvite>();
            if (profile == null)
                return friendsToInvite;

            List<Profile> profiles = DBContext.Profiles.Where(p => p.Id != id).OrderByDescending(p => p.LastName).ToList();
            foreach (var p in profiles)
            {
                FriendToInvite friendToInvite = new FriendToInvite();
                friendToInvite.Id = (Guid)p.Id;

                friendToInvite.ProfileUrl = $"{profile.Name}{profile.LastName}";
                friendToInvite.Name = p.Name;
                friendToInvite.LastName = p.LastName;
                if (p.ImageId != null)
                {
                    var image = DBContext.Images.Where(i => i.Id == p.ImageId).FirstOrDefault();
                    friendToInvite.ImageUrl = image.UrlImage;
                }
                friendsToInvite.Add(friendToInvite);

            }

            return friendsToInvite;
        }

        public object GetAllFriends(Guid id)
        {
            Profile profile = DBContext.Profiles.Where(p => p.Id == id).FirstOrDefault();
            List<FriendToInvite> friendsToInvite = new List<FriendToInvite>();
            if (profile == null)
                return friendsToInvite;

            List<Friend> friends = DBContext.Friends.Where(f => f.UserOneId == id || f.UserTwoId == id).ToList();

            foreach (var f in friends)
            {
                Profile friendProfile;
                if (f.UserOneId == id)
                {
                    friendProfile = DBContext.Profiles.Where(p => p.Id == f.UserTwoId).FirstOrDefault();
                }
                else
                {
                    friendProfile = DBContext.Profiles.Where(p => p.Id == f.UserOneId).FirstOrDefault();
                }

                FriendToInvite friendToInvite = new FriendToInvite();
                friendToInvite.Id = (Guid)friendProfile.Id;

                friendToInvite.ProfileUrl = $"{profile.Name}{profile.LastName}";
                friendToInvite.Name = friendProfile.Name;
                friendToInvite.LastName = friendProfile.LastName;
                if (friendProfile.ImageId != null)
                {
                    friendProfile.Image = DBContext.Images.Where(i => i.Id == friendProfile.ImageId).FirstOrDefault();
                    friendToInvite.ImageUrl = friendProfile.Image.UrlImage;

                }
                friendsToInvite.Add(friendToInvite);

            }

            return friendsToInvite;
        }

        public object GetSellerLevelByProfileId(Guid id)
        {
            Profile profile = DBContext.Profiles.Where(p => p.Id == id).FirstOrDefault();
            if (profile == null)
                return new { error = "No se encuentra ningun perfil con la id ingresada" };

            int sells = DBContext.Sells.Where(s => s.SellerId == id).Count();
            if (sells < 10)
                return 1;
            if (sells >= 10 && sells < 50)
                return 2;
            if (sells >= 50 && sells < 100)
                return 3;
            if (sells >= 100 && sells <= 200)
                return 4;
            if (sells > 200)
                return 5;

            return sells;
        }

        public object AcceptFriendRequest(Guid id)
        {
            FriendRequest friendRequest = DBContext.FriendRequests.Where(f => f.Id == id).FirstOrDefault();
            if (friendRequest == null)
                return new { error = "No existe ninguna solicitud de amistad con esta id " };

            if (friendRequest.Accepted == true)
                return new { error = "La solicitud ya fue aceptada" };

            if (friendRequest.Deleted == true)
                return new { error = "La solicitud ya fue eliminada" };

            var repeatedFriend = DBContext.Friends.Where(friend => friend.UserOneId == friendRequest.FromId && friend.UserTwoId == friendRequest.ToId).FirstOrDefault();
            if (repeatedFriend != null && repeatedFriend.Deleted == false)
                return new { error = "Estos usuarios ya son amigos" };

            // create new friend
            friendRequest.Accepted = true;
            DBContext.Entry(friendRequest).State = EntityState.Modified;
            DBContext.SaveChanges();

            Friend friend = new Friend();
            friend.UserOneId = friendRequest.FromId;
            friend.UserTwoId = friendRequest.ToId;
            friend.Id = Guid.NewGuid();
            friend.Deleted = false;
            DBContext.Friends.Add(friend);
            DBContext.SaveChanges();

            return friendRequest;
        }


        public object GetCart(Guid id)
        {
            Cart cart = DBContext.Carts.Where(c => c.ProfileId == id).FirstOrDefault();
            if (cart == null)
            {
                cart = new Cart();
                cart.Id = Guid.NewGuid();
                cart.ProfileId = id;
                cart.Deleted = false;
                cart.Total = 0;
                DBContext.Carts.Add(cart);
                DBContext.SaveChanges();
            }
            cart.CartPublications = DBContext.CartPublications.Where(cp => cp.CartId == cart.Id && cp.Deleted != true).ToList();
            cart.CartPublications.ForEach(cp => cp.Publication = DBContext.Publications.Where(p => p.Id == cp.PublicationId).FirstOrDefault());

            return cart;
        }

        public object AddToCart(Guid id, Guid publicationId, int amount)
        {
            Cart cart = DBContext.Carts.Where(c => c.Id == id).FirstOrDefault();
            if (cart == null)
                return new { error = "No se encuentra ningun carrito con este id" };

            if (cart.Deleted == true)
                return new { error = "El carrito ya fue eliminado" };

            if (amount <= 0)
                return new { error = "La cantidad debe ser mayor a cero" };

            Publication publication = DBContext.Publications.Where(p => p.Id == publicationId).FirstOrDefault();
            if (publication == null)
                return new { error = "No se encuentra ninguna publicación con esta id" };

            CartPublication cartPublication = DBContext.CartPublications.Where(cp => cp.CartId == cart.Id && cp.PublicationId == publicationId).FirstOrDefault();
            CartPublication newCartPublication;

            if (cartPublication != null)
            {
                cartPublication.Amount += amount;
                cartPublication.Deleted = false;

                newCartPublication = cartPublication;

            }
            else
            {
                newCartPublication = new CartPublication();
                newCartPublication.Id = Guid.NewGuid();
                newCartPublication.CartId = cart.Id;
                newCartPublication.PublicationId = publicationId;
                newCartPublication.Amount = amount;
                newCartPublication.Deleted = false;
                DBContext.CartPublications.Add(newCartPublication);
            }
            cart.Total += publication.Price * amount;
            cart.ModificationDateTime = DateTime.Now;
            DBContext.SaveChanges();

            return newCartPublication;
        }

        public object DeleteCartPublication(Guid id)
        {
            CartPublication cartPublication = DBContext.CartPublications.Where(cp => cp.Id == id).FirstOrDefault();
            if (cartPublication == null)
                return new { error = "No se encuentra ninguna publicación en el carrito" };

            cartPublication.Publication = DBContext.Publications.Where(p => p.Id == cartPublication.PublicationId).FirstOrDefault();

            cartPublication.Deleted = true;
            Cart cart = DBContext.Carts.Where(c => c.Id == cartPublication.CartId).FirstOrDefault();
            cart.Total -= cartPublication.Amount * cartPublication.Publication.Price;

            if (cart.Total < 0) cart.Total = 0;
            cart.ModificationDateTime = DateTime.Now;
            cartPublication.Amount = 0;

            DBContext.Entry(cartPublication).State = EntityState.Modified;
            DBContext.SaveChanges();

            return cartPublication;
        }

        public object EditCartPublication(Guid id, int amount)
        {
            CartPublication cartPublication = DBContext.CartPublications.Where(cp => cp.Id == id).FirstOrDefault();
            if (cartPublication == null)
                return new { error = "No se encuentra ninguna publicación en el carrito" };

            if (amount <= 0)
                return DeleteCartPublication(id);

            cartPublication.Publication = DBContext.Publications.Where(p => p.Id == cartPublication.PublicationId).FirstOrDefault();

            Cart cart = DBContext.Carts.Where(c => c.Id == cartPublication.CartId).FirstOrDefault();
            cart.Deleted = false;
            cart.Total -= cartPublication.Publication.Price * (cartPublication.Amount - amount);
            cartPublication.Amount = amount;

            if (cart.Total < 0) cart.Total = 0;

            cart.ModificationDateTime = DateTime.Now;

            DBContext.Entry(cartPublication).State = EntityState.Modified;
            DBContext.SaveChanges();

            return cartPublication;
        }



        public object SearchContacts(string query, Guid userId)
        {
            User user = DBContext.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user.ProfileId == null)
            {
                return new { error = "No se encuentra ningun perfil asociado a este usuario" };
            }
            List<Contact> contacts = (List<Contact>)DBContext.Contacts
                        .Include(c => c.ContactProfile)
                        .Include(c => c.ContactProfile.Image)
                        .Where(c => c.ProfileId == user.ProfileId
                                && (
                                (query.Contains(c.ContactProfile.Name) || query.Contains(c.ContactProfile.LastName))
                                ||
                                (c.ContactProfile.ProfileUrl.Contains(query))
                                ||
                                (c.ContactProfile.Phone.Contains(query))
                                )
                                && c.ContactProfile.ProfileUrl != "TopmaiPay"
                                && c.ContactProfile.ProfileUrl != "SoporteTopmai"

                                ).ToList();
            return contacts;
        }
        public object Search(string query, Guid userId)
        {
            if (query == null)
                return new { error = "Ingrese un nombre o celular a buscar" };
            var search = query.Split(" ");
            List<Profile> profiles = new List<Profile>();
            List<string> contacts = DBContext.Contacts.Where(c => c.ProfileId == userId)
                                                      .Select(c => c.ContactProfileId.ToString()).ToList();
            foreach (var s in search)
            {
                var p = DBContext.Profiles.Where(p => (
                                                  (s.Contains(p.Name)
                                                 && s.Contains(p.LastName))
                                                || (p.Phone == s)
                                                || (p.ProfileUrl == s)
                                                      ) && (p.Id != userId) && p.Deleted != true).ToList();
                foreach (var pf in p)
                {
                    bool isContact = false;
                    foreach (var c in contacts)
                    {
                        if (c.Contains(pf.Id.ToString().ToUpper())) isContact = true;
                    }
                    if (!isContact)
                    {
                        pf.Image = DBContext.Images.Where(i => i.Id == pf.ImageId).FirstOrDefault();
                        profiles.Add(pf);
                    }
                }

            }

            return profiles;
        }


        public object Delete(Guid id)
        {

            Profile profile = DBContext.Profiles.FirstOrDefault(u => u.Id == id);
            if (profile == null) return new { error = "El id ingresado no es válido" };

            profile.Deleted = true;
            DBContext.Entry(profile).State = EntityState.Modified;
            DBContext.SaveChanges();
            return profile;
        }

        #endregion
    }

    internal class FriendToInvite
    {
        public Guid Id { get; set; }
        public string ProfileUrl { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
