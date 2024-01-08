namespace Infraestructure.Entity.Request
{
    public class PaymentRequest
    {
        public string email { get; set; }
        public int amount { get; set; }

        public Guid userId { get; set; }
    }
}