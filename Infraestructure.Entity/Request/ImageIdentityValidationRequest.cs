namespace Infraestructure.Entity.Request
{
    public class ImageIdentityValidationRequest
    {
        public Guid idImage { get; set; }
        public Guid idIdentityValidation { get; set; }
        public string? type { get; set; }
    }
}