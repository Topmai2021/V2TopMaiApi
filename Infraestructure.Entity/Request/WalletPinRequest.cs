namespace Infraestructure.Entity.Request
{
    public class WalletPinRequest
    {
        public Guid userId { get; set; }
        public int pin { get; set; }
        public int? oldPin { get; set; }
    }
}