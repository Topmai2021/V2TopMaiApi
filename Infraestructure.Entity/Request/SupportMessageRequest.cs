namespace Infraestructure.Entity.Request
{
    public class SupportMessageRequest
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }
    }
}