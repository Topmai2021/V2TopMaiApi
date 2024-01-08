namespace Infraestructure.Entity.Entities.Profiles
{
    public partial class Contact
    {
        public Guid Id { get; set; }
        public Guid ProfileId { get; set; }
        public DateTime DateTime { get; set; }
        public string? Phone { get; set; }
        public Guid ContactProfileId { get; set; }
        public bool Locked { get; set; }

        public virtual Profile ContactProfile { get; set; } = null!;
    }
}