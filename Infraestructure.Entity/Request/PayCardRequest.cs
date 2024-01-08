namespace Infraestructure.Entity.Request
{
    public class PayCardRequest
    {
        public Guid profileId { get; set; }
        public Guid? cardId { get; set; }
        public decimal total { get; set; }
        public string? deviceSessionId { get; set; }
    }
}