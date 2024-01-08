namespace TopMai.Domain.DTO.Chats
{
    public class VerifyChatDto
    {
        public Guid IdProfileSender { get; set; }
        public Guid? PublicationId { get; set; }
        public Guid IdProfileReceiver { get; set; }
    }
}
