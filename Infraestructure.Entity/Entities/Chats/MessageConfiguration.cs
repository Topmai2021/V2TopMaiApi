using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Chats
{
    public partial class MessageConfiguration
    {
        public Guid Id { get; set; }
        public bool? MessageDeleted { get; set; }
        public Guid? MessageId { get; set; }
        public Guid? ProfileId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? Deleted { get; set; }

        public virtual Message? Message { get; set; }
        public virtual Profile? Profile { get; set; }
    }
}