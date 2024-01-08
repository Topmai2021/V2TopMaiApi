using Common.Utils.Enums;
using Common.Utils.Exceptions;
using Common.Utils.Helpers;
using Common.Utils.Resources;
using EasyPost;
using Infraestructure.Core.Dapper;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Users;
using Infraestructure.Entity.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NETCore.Encrypt;
using OneSignal.RestAPIv3.Client.Resources.Devices;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.CodeValidation;
using TopMai.Domain.DTO.Profiles;
using TopMai.Domain.DTO.User;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Domain.Services.Products.Interfaces;
using TopMai.Domain.Services.Profiles.Interfaces;
using TopMai.Domain.Services.Users.Interfaces;
using static Common.Utils.Constant.Const;
using static Common.Utils.Enums.Enums;
using User = Infraestructure.Entity.Entities.Users.User;

namespace TopMai.Domain.Services.Users
{
    public class UserService : IUserService
    {
        #region Attributes
        private readonly IUnitOfWork _unitOfWork;

        private readonly IPublicationService _publicationService;
        private readonly IWalletService _walletService;
        private readonly IProfileService _profileService;
        private readonly IRoleService _roleService;
        private readonly IImageService _imageService;
        private readonly ICodeValidationServices _codeValidation;
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
		#endregion

		#region Builder
		public UserService(IUnitOfWork unitOfWork,
			IWalletService walletService,
			IPublicationService PublicationService,
			IRoleService roleService,
			IImageService imageService,
			ICodeValidationServices codeValidation,
			IConfiguration configuration,
			DataContext dataContext,
			IProfileService profileService)

		{
			_unitOfWork = unitOfWork;
			_walletService = walletService;
            _profileService = profileService;
            _publicationService = PublicationService;
			_roleService = roleService;
			_imageService = imageService;
			_codeValidation = codeValidation;
			_configuration = configuration;
			_dataContext = dataContext;
			_profileService = profileService;
		}
		#endregion

		#region Methods
		public async Task<(List<User>, int totalCount)> GetAll(int page = 1, int limit=10)
        {
            int skip = (page - 1) * limit;


            List<User> users = _unitOfWork.UserRepository.FindAll(
                x => x.RoleId != null,
                x => x.Profile,
                x => x.Profile.Wallet,
                r => r.Role).OrderByDescending(x => x.RegisterDate)
                .Skip(skip)
                .Take(limit)
                .ToList();

            int totalCount = _dataContext.Users.Count();


            List<Profile?> profilesWithoutWallet = users
                          .Where(x => x.ProfileId != null
                          && x.Profile.WalletId == null)
                          .Select(x => x.Profile).ToList();

            if (profilesWithoutWallet.Count > 0)
            {
                foreach (Profile profile in profilesWithoutWallet)
                {
                    Wallet wallet = new Wallet();
                    wallet.Id = Guid.NewGuid();
                    wallet.CurrencyId = (int)Enums.Currency.MXN;
                    wallet.Money = 0;
                    await _walletService.Post(wallet);
                    profile.WalletId = wallet.Id;
                    await _profileService.Put(profile);
                }
            }

            return (users, totalCount);
        }

        public async Task<User> ChangeRole(Guid userId, int roleId)
        {
            User user = _unitOfWork.UserRepository.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                throw new BusinessException("Usuario no encontrado");

            if (user.RoleId == (int)Enums.Rol.Admin)
                throw new BusinessException("No se puede cambiar el rol del usuario admin");

            Role role = _roleService.Get(roleId);
            if (role == null)
                throw new BusinessException("Rol no encontrado");

            user.RoleId = roleId;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Save();

            return user;
        }

        public UserDto Get(Guid id)
        {
            User user = _unitOfWork.UserRepository.FirstOrDefault(u => u.Id == id,
                                                                  x => x.Profile,
                                                                  x => x.Profile.Image);

            if (user != null)
            {
                UserDto userDto = new UserDto()
                {
                    IdUser = id,
                    Mail = user.Mail,
                    Phone = user.Phone,
                    RoleId = user.RoleId,
                    UserName = user.UserName,
                    FullName = user?.Profile?.FullName,
                    IdWallet = user?.Profile?.WalletId,
                    ProfileId = user.ProfileId,
                    UrlImage = _imageService.GetUrlImage(user?.Profile?.Image),
                    StrRole = Helper.GetRole(user.RoleId),
                };

                return userDto;
            }

            throw new BusinessException("Usuario no se encuentra registrado en el sistema");
        }

        public User GetUser(Guid id) => _unitOfWork.UserRepository.FirstOrDefault(u => u.Id == id);
        public User GetUserByEmail(string mail) => _unitOfWork.UserRepository.FirstOrDefault(u => u.Mail.ToLower() == mail.ToLower());
        public User GetUserByPhone(string phone) => _unitOfWork.UserRepository.FirstOrDefault(u => u.Phone.ToLower() == phone.ToLower());

        public async Task<TokenDto> Post(AddUser_Dto? user)
        {
            ValidatePassword(user.Password);

            if (user.UserName != null && user.UserName.Length < 4)
                throw new BusinessException("El nombre de usuario debe poseer al menos 4 digitos");

            if (!string.IsNullOrEmpty(user.Mail) && !IsMail(user.Mail))
                throw new BusinessException("El e-mail ingresado no es válido");

            if (!string.IsNullOrEmpty(user.Phone) && !IsPhone(user.Phone))
                throw new BusinessException("El número de celular ingresado no es válido");

            if (user.UserName == null && user.Mail == null && user.Phone == null)
                throw new BusinessException("Debe ingresar una forma de acceso");

            if (user.Mail != null && !string.IsNullOrEmpty(user.Phone))
                throw new BusinessException("No puede ingresar más de una forma de acceso ");

            User userResult = null;
            if (string.IsNullOrEmpty(user.Mail))
            {
                userResult = _unitOfWork.UserRepository.FirstOrDefault(u => u.UserName.Equals(user.UserName) || u.Phone.ToLower() == user.Phone.ToLower());


                if (userResult != null)
                    throw new BusinessException("El nombre de usuario y/o teléfono ya se encuentran registrados en el sistema");
            }
            else
            {
                userResult = _unitOfWork.UserRepository.FirstOrDefault(u => (u.UserName == user.UserName)
                                                                        || (u.Mail.ToLower() == user.Mail.ToLower()));

                if (userResult != null)
                    throw new BusinessException("El nombre de usuario y/o e-mail ya se encuentran registrados en el sistema");

            }

            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Password = EncryptProvider.Sha1(user.Password),
                RegisterDate = DateTime.Now,
                RoleId = (int)Enums.Rol.Default,
                Mail = user.Mail,
                Phone = user.Phone,
                UserName = user.UserName == null ? "TopMaiUser" : user.UserName,
            };

            if (user.Mail != null || !string.IsNullOrEmpty(user.Phone))
                newUser.Validated = false;
            else
                newUser.Validated = true;

            return await SaveUser(newUser, $"{user.Mail}{user.Phone}{user.UserName}");
        }

        public async Task<TokenDto> SocialRegister(SocialRegister socialRegister)
        {
            if (socialRegister.SocialUserId is null || socialRegister.SignupType is SignupTypeEnum.Unknown || socialRegister.Email is null)
                throw new BusinessException(GeneralMessages.FieldNotDefined);

            if (!IsMail(socialRegister.Email))
                throw new BusinessException("El e-mail ingresado no es válido");

            User userResult = _unitOfWork.UserRepository.FirstOrDefault(u => u.Mail != null && u.Mail.Equals(socialRegister.Email));

            if (userResult != null)
                throw new BusinessException("El usuario ya se encuentran registrados en el sistema");

            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                Deleted = false,
                Password = EncryptProvider.Sha1(socialRegister.SocialUserId),
                RegisterDate = DateTime.Now,
                RoleId = (int)Enums.Rol.Default,
                Mail = socialRegister.Email,
                Phone = null,
                UserName = "TopMaiUser",
                Validated = true,
                SignupType = Convert.ToInt16(socialRegister.SignupType),
                VerifiedEmail = true
            };

            return await SaveUser(newUser, $"{newUser.Mail}{newUser.UserName}");
        }

        private async Task<TokenDto> SaveUser(User newUser, string userName)
        {
            _unitOfWork.UserRepository.Insert(newUser);
            await _unitOfWork.Save();

            return GenerateTokenJWT(newUser, $"{newUser.Mail}{newUser.UserName}");
        }


        public async Task ChangeForgottenPassword(ChangeForgottenPasswordDto change)
        {
            if (change.UserLogin is null || change.Password is null || change.Type == Enums.TypeCodeValidation.Unknown)
            {
                throw new BusinessException("Please provide all informations.");
            }

            ValidatePassword(change.Password);
            var validateCode = new ChangeForgottenPassword()
            {
                UserLogin = change.UserLogin,
                Code = change.Code,
                Type = change.Type
            };
            
            var isValid = await _codeValidation.ValidateForgottenCode(validateCode);

            if (!isValid)
                throw new BusinessException("No se pudo validar el código, por favor intentarlo de nuevo");

            UpdatePassword(EncryptProvider.Sha1(change.Password), change.UserLogin, change.Type);
        }

        private void UpdatePassword(string password, string userLogin, TypeCodeValidation type)
        {
            if (type == TypeCodeValidation.Email)
            {
                UpdatePasswordByEmail(EncryptProvider.Sha1(password), userLogin);
            }
            else if (type == TypeCodeValidation.Phone)
            {
                UpdatePasswordByEmail(EncryptProvider.Sha1(password), userLogin);
            }
            else
            {
                throw new BusinessException("No hay un usuario especificado para este type");
            }
        }

        private void UpdatePasswordByEmail(string encryptedPassword, string userEmail)
        {
            DapperHelper.Instancia.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var filter = new
            {
                password = encryptedPassword,
                mail = userEmail
            };
            DapperHelper.Instancia.ExecuteQueryScalar(StatementSql.updatePasswordByMail, filter);
        }


        private void UpdatePasswordByPhone(string encryptedPassword, string userPhone)
        {
            DapperHelper.Instancia.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var filter = new
            {
                password = encryptedPassword,
                mail = userPhone
            };
            DapperHelper.Instancia.ExecuteQueryScalar(StatementSql.updatePasswordByPhone, filter);
        }

        public object Put(UpdateUserRequest newUser)
        {
            if (newUser?.Id == null || newUser.Id.ToString().Length < 6)
                throw new BusinessException("Ingrese un id de usuario válido ");

            User? user = _unitOfWork.UserRepository.FirstOrDefault(u => u.Id == newUser.Id);

            if (user == null)
                throw new BusinessException("El id no coincide con ningun usuario");

            string lastPassword = user.Password;

            if (newUser.Password != lastPassword && newUser.Password != null)
                newUser.Password = EncryptProvider.Sha1(newUser.Password);

            user.Validated = newUser.Validated;

            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.SaveChanges();
            return user;
        }


public Dashboard GetAdminDashboardData()
{
    Dashboard data = new Dashboard();

    // Get the current year and month
    DateTime currentDate = DateTime.Now;

    // Fetch all publications count in a single database query
    data.PublicationCount = _dataContext.Publications.Count();

    // Fetch user registration counts for the current year
    var userCountsByMonth = new int[12];

    var users = _unitOfWork.UserRepository
        .FindAll(x => x.Deleted != true)
        .ToList();

    foreach (var user in users)
    {
        int month = user.RegisterDate.Month;
        userCountsByMonth[month - 1]++; // Month is 1-based
    }

    // Set the user count and user counts by month
    data.UserCount = users.Count();
    data.ComplaintCount = _dataContext.Complaints.Count();
    data.UserCountsByMonth = userCountsByMonth;

    // Fetch sells data
    int[] monthlySells = new int[12];
    int totalSells = _dataContext.Sells.Count();

    for (int i = 0; i < 12; i++)
    {
        int currentMonth = i + 1;
        monthlySells[i] = _dataContext.Sells
            .Where(s => s.DateTime != null && s.DateTime.Value.Month == currentMonth)
            .Count();
    }

    data.TotalSells = totalSells;
    data.MonthlySells = monthlySells;

    return data;
}


        public TokenDto Login(string userName, string password)
        {
            string typeLogin = getTypeUserName(userName);
            if (!string.IsNullOrEmpty(typeLogin))
            {
                password = EncryptProvider.Sha1(password);

                User userLogin;
                if (typeLogin == "phone")
                    userLogin = LoginWithPhone(userName, password);
                else if (typeLogin == "mail")
                    userLogin = LoginWithMail(userName, password);
                else
                    userLogin = LoginWithUser(userName, password);

                if (userLogin == null)
                    throw new BusinessException(GeneralMessages.BadCredentials);


                return GenerateTokenJWT(userLogin, userName);
            }
            else
                throw new BusinessException("Usuario no válido");
        }

        public TokenDto SocialLogin(string? socialUserId, string? email)
        {
            if (socialUserId is null || email is null)
                throw new BusinessException(GeneralMessages.BadCredentials);

            var password = EncryptProvider.Sha1(socialUserId);
            var userLogin = LoginWithMail(email, password);

            if (userLogin is null)
                throw new BusinessException(GeneralMessages.UserNotFound);

            return GenerateTokenJWT(userLogin, email);
        }

        public async Task<bool> UpdateUser(User userUpdate)
        {
            _unitOfWork.UserRepository.Update(userUpdate);
            return await _unitOfWork.Save() > 0;
        }


        #region Privados


        private void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                throw new BusinessException("La contraseña debe ser de al menos 6 digitos");

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");

            if ((!hasNumber.IsMatch(password) || !hasUpperChar.IsMatch(password)))
                throw new BusinessException("La contraseña debe contener al menos un numero y una letra mayuscula");
        }

        private TokenDto GenerateTokenJWT(User userEntity, string userName)
        {
            IConfigurationSection tokenAppSetting = _configuration.GetSection("Tokens");

            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenAppSetting.GetSection("Key").Value));
            var _signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var _header = new JwtHeader(_signingCredentials);

            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(TypeClaims.IdUser, userEntity.Id.ToString()),
                new Claim(TypeClaims.UserName, userName),
                new Claim(TypeClaims.IdRol, userEntity.RoleId.ToString()),
                new Claim(TypeClaims.Rol, Helper.GetRole(userEntity.RoleId)),
            };

            var _payload = new JwtPayload(
                    issuer: tokenAppSetting.GetSection("Issuer").Value,
                    audience: tokenAppSetting.GetSection("Audience").Value,
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddDays(720)
                );

            var _token = new JwtSecurityToken(
                    _header,
                    _payload
                );

            TokenDto token = new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(_token),
                Expiration = Helper.ConvertToUnixTimestamp(_token.ValidTo),
                IdUser = userEntity.Id,
                Role = Helper.GetRole(userEntity.RoleId),
                IsAdmin = userEntity.RoleId == (int)Enums.Rol.Admin
            };

            if (userEntity.ProfileId != null)
                token.IdProfile = userEntity.ProfileId.Value;

            return token;
        }

        private bool IsPhone(string userName)
        {
            var phoneNumber = userName.Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "");
            return Regex.Match(phoneNumber, @"^\+\d{5,15}$").Success;

        }

        private string getTypeUserName(string userName)
        {
            if (IsPhone(userName))
                return "phone";
            else
            {
                if (IsMail(userName))
                    return "mail";
                else
                {
                    if ((userName.Length >= 4 && userName.Length <= 12) || (userName.Contains("facebook") || userName.Contains("google")))
                        return "user";
                    else
                        return String.Empty;

                }
            }
        }

        private bool IsMail(string userName)
        {
            return (new EmailAddressAttribute().IsValid(userName));
        }

        public User LoginWithPhone(string phone, string password)
        {
            return _unitOfWork.UserRepository.FirstOrDefault(x => x.Phone == phone
                                                               && x.Password == password
                                                               && x.Deleted == false,
                                                              r => r.Role);
        }

        public User LoginWithMail(string mail, string password)
        {
            return _unitOfWork.UserRepository.FirstOrDefault(x => x.Mail == mail
                                                                        && x.Password == password
                                                                        && !x.Deleted);
        }

        public User LoginWithUser(string userName, string password)
        {
            if (userName == "000apple")
            {
                return _unitOfWork.UserRepository.FirstOrDefault(x => x.UserName.Contains("apple")
                                                                        && x.Password == password
                                                                        && x.Deleted == false,
                                                              r => r.Role);
            }

            return _unitOfWork.UserRepository.FirstOrDefault(x => x.UserName == userName
                                                                   && x.Password == password
                                                                   && x.Deleted == false,
                                                              r => r.Role);
        }

        #endregion

        public User VerifyEmail(string email)
        {
            if (IsMail(email))
            {
                User user = _unitOfWork.UserRepository.FirstOrDefault(u => u.Mail == email);
                if (user == null)
                    throw new BusinessException("El email ingresado no pertenece a ningun usuario");
                return user;
            }
            else
                throw new BusinessException("El email ingresado no es valido");
        }

        public async Task<User> ChangePassword(ChangePasswordRequest changePassword)
        {
            changePassword.lastPassword = EncryptProvider.Sha1(changePassword.lastPassword);
            changePassword.newPassword = EncryptProvider.Sha1(changePassword.newPassword);

            User users = _unitOfWork.UserRepository.FirstOrDefault(x => x.Id == changePassword.id);

            if (changePassword.lastPassword == users.Password)
            {
                users.Password = changePassword.newPassword;

                _unitOfWork.UserRepository.Update(users);
                await _unitOfWork.Save();

                return Task.FromResult(users).Result;
            }
            throw new BusinessException("La contraseña actual no es la correcta");
        }


        public async Task<Profile> CreateProfile(AddProfile_Dto profile)
        {
            User user = GetUser(profile.Id);
            if (user == null)
                throw new BusinessException("No se encuentró ningún usuario relacionado");

            using (var db = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    profile.Id = user.Id;
                    Profile newProfile = await _profileService.Post(profile);
                    user.ProfileId = newProfile.Id;
                    _unitOfWork.UserRepository.Update(user);
                    await _unitOfWork.Save();

                    await db.CommitAsync();
                    return newProfile;
                }
                catch (Exception ex)
                {
                    await db.RollbackAsync();

                    throw new Exception("Hubo un error al realizar la operación, por favor vuelta a intentarlo", ex);
                }
            }

        }

         public async Task<object> Delete(Guid id)
        {

          User user = _unitOfWork.UserRepository.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return user;
            }

            if(user.Deleted==true){
               _unitOfWork.UserRepository.Delete(user);
            }else{
             user.Deleted = true;
              _unitOfWork.UserRepository.Update(user);
            }
  
              _unitOfWork.SaveChanges();
           
            return user;
        }
         #endregion
    }
}
