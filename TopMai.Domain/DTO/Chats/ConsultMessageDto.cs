using TopMai.Domain.DTO.Profiles;

namespace TopMai.Domain.DTO.Chats
{
    public class ConsultMessageDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime DateHour { get; set; }
        public Guid FromId { get; set; }
        public bool Readed { get; set; }
        public Guid ChatId { get; set; }
        public int MessageTypeId { get; set; }

        public ConsultProfileDto? From { get; set; }
    }
}
