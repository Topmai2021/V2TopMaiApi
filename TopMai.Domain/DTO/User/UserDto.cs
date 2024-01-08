namespace TopMai.Domain.DTO.User
{
    public class UserDto
    {
        public Guid IdUser { get; set; }
        public Guid? ProfileId { get; set; }
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Mail { get; set; }
        public int RoleId { get; set; }
        public string StrRole { get; set; }
        public string? UrlImage { get; set; }
        public Guid? IdWallet { get; set; }
    }
}
