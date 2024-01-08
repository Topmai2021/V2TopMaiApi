using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class Card
    {
        public Guid Id { get; set; }
        public long Number { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public string FullName { get; set; } = null!;
        public string SecurityCode { get; set; } = null!;
        public string? Type { get; set; }
        public Guid? ProfileId { get; set; }
        public bool? Deleted { get; set; }
        public string? OpenPayCardId { get; set; }

        public virtual Profile? Profile { get; set; }
    }
}