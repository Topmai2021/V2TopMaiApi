namespace Infraestructure.Entity.Request
{
    public class CartRequest
    {
        public Guid id { get; set; }
        public Guid? publicationId { get; set; }
        public int amount { get; set; }
    }
}