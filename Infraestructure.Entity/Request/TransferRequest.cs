namespace Infraestructure.Entity.Request
{
    public class TransferRequest
    {
        public Guid idFrom { get; set; }
        public Guid idTo { get; set; }
        public float amount { get; set; }
    }
}