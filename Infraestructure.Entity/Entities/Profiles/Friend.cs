namespace Infraestructure.Entity.Entities.Profiles
{
    public partial class Friend
    {
        public Guid Id { get; set; }
        public Guid? UserOneId { get; set; }
        public Guid? UserTwoId { get; set; }
        public bool? Deleted { get; set; }
    }
}