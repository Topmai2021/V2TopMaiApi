namespace Infraestructure.Entity.Request
{
    public class PublicationImageRequest
    {
        public Guid idImage { get; set; }
        public Guid idPublication { get; set; }

        public int? number { get; set; }
    }
}