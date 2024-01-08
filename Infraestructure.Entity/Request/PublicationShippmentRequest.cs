namespace Infraestructure.Entity.Request
{
    public class PublicationShippmentRequest
    {
        public Guid idPublication { get; set; }
        public int idShippmentType { get; set; }

        public float? price { get; set; }
    }
}