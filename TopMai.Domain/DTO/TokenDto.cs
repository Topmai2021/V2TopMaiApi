namespace TopMai.Domain.DTO
{
    public class TokenDto
    {
        public string? Token { get; set; }
        public double Expiration { get; set; }
        public bool IsAdmin { get; set; }
        public Guid  IdUser { get; set; }
        public Guid?  IdProfile { get; set; }
        public string? Role { get; set; }
    }
}
