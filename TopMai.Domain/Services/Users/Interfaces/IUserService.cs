using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Users;
using Infraestructure.Entity.Request;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Profiles;
using TopMai.Domain.DTO.User;

namespace TopMai.Domain.Services.Users.Interfaces
{
    public interface IUserService
    {
        Task<(List<User>, int totalCount)> GetAll(int page = 1, int limit = 10);
        UserDto Get(Guid id);
        User GetUser(Guid id);
        User GetUserByEmail(string mail);
        User GetUserByPhone(string phone);
        Task<TokenDto> Post(AddUser_Dto user);
        Task<TokenDto> SocialRegister(SocialRegister socialRegister);
        object Put(UpdateUserRequest newUser);
            Dashboard GetAdminDashboardData();          
        TokenDto Login(string userName, string password);
        TokenDto SocialLogin(string? socialUserId, string? email);
        User VerifyEmail(string email);
        Task ChangeForgottenPassword(ChangeForgottenPasswordDto change);
        Task<User> ChangePassword(ChangePasswordRequest changePassword);
        Task<Profile> CreateProfile(AddProfile_Dto profile);
        Task<object> Delete(Guid id);

        Task<User> ChangeRole(Guid userId, int roleId);

        Task<bool> UpdateUser(User userUpdate);
    }
}
