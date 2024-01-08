using TopMai.Domain.DTO.Profiles;

namespace TopMai.Domain.DTO.Chats
{
    public class ChatDto
    {
        public int NewMessagesAmount { get; set; }
        public ConsultMessageDto? LastMessage { get; set; }
        public ChatConfigurationDto ChatConfiguration { get; set; } = null!;
        public ConsultProfileDto ProfileSender { get; set; } = null!;
        public ConsultProfileDto ProfileReceiver { get; set; } = null!;
    }
}
