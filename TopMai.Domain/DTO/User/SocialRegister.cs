using Infraestructure.Entity.Entities.Users;

namespace TopMai.Domain.DTO.User
{
    public class SocialRegister
    {
        public string? SocialUserId {  get; set; }
        public string? Email { get; set; }
        public SignupTypeEnum SignupType { get; set; }
    }
}
