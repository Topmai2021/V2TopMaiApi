namespace Infraestructure.Entity.Entities.Users
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public string Name { get; set; } = null!;
        public bool Deleted { get; set; }
        public int Id { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}