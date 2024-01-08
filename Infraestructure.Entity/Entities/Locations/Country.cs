using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Locations
{
    public partial class Country
    {
        public Country()
        {
            Profiles = new HashSet<Profile>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }
    }
}