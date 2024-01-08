namespace TopMai.Domain.DTO.Chats
{
    public class ChatConfigurationDto
    {
        public Guid ChatId { get; set; }
        public Guid ProfileId { get; set; }
        public bool Silenced { get; set; }
        public bool AlwaysUp { get; set; }
        public bool Blocked { get; set; }
        public string? BackgroundUrl { get; set; }
    }
}
