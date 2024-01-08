using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class CardType
    {
        public CardType()
        {
            ProfileCards = new HashSet<ProfileCard>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<ProfileCard> ProfileCards { get; set; }
    }
}