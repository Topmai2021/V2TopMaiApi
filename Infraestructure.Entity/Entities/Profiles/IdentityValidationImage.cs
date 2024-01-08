namespace Infraestructure.Entity.Entities.Profiles
{
    public partial class IdentityValidationImage
    {
        public Guid Id { get; set; }
        public Guid? ImageId { get; set; }
        public Guid? IdentityValidationId { get; set; }
        public string? Type { get; set; }
        public bool? Deleted { get; set; }

        public virtual IdentityValidation? IdentityValidation { get; set; }
        public virtual Image? Image { get; set; }
    }
}