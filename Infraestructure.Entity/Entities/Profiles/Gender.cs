namespace Infraestructure.Entity.Entities.Profiles
{
    public partial class Gender
    {
        public Gender()
        {
            Profiles = new HashSet<Profile>();
        }

        public string Name { get; set; } = null!;
        public int Id { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }
    }
}