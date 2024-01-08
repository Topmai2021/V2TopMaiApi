namespace Infraestructure.Entity.Entities.Profiles
{
    public partial class FriendRequest
    {
        public Guid Id { get; set; }
        public Guid? FromId { get; set; }
        public Guid? ToId { get; set; }
        public bool? Accepted { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? DateTime { get; set; }
    }
}