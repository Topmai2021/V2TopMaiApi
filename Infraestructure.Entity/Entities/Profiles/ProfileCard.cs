using Infraestructure.Entity.Entities.Payments;

namespace Infraestructure.Entity.Entities.Profiles
{
    public partial class ProfileCard
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool? Deleted { get; set; }
        public Guid? ProfileId { get; set; }
        public string? Number { get; set; }
        public Guid? CardTypeId { get; set; }

        public virtual CardType? CardType { get; set; }
        public virtual Profile? Profile { get; set; }
    }
}