namespace Infraestructure.Entity.Request
{
    public class SellOfferRequest
    {
        public Guid sellRequestId { get; set; }
        public Guid userId { get; set; }
        public int paymentMethodId { get; set; }
    }
}