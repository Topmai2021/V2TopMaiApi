using System.ComponentModel;

namespace TopMai.Domain.DTO.User
{
    public class AddUser_Dto
    {
        [DefaultValue(null)]
        public string? Password { get; set; }
        [DefaultValue(null)]
        public string? UserName { get; set; }
        [DefaultValue(null)]
        public string? Phone { get; set; }
        [DefaultValue(null)]
        public string? Mail { get; set; }
    }
}
