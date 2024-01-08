namespace Infraestructure.Entity.Request
{
    public class PublicationRequest
    {
        public Guid publicationId { get; set; }
        public float total { get; set; }
        public bool? withShippment { get; set; }
    }
}