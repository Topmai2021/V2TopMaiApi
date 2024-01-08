using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Chats
{
    public partial class ChatConfiguration
    {
        public Guid Id { get; set; }
        public bool? ChatDeleted { get; set; }
        public Guid? ChatId { get; set; }
        public Guid? ProfileId { get; set; }
        public string? BackgroundUrl { get; set; }
        public bool? Silenced { get; set; }
        public bool? AlwaysUp { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? Deleted { get; set; }
        public bool? Blocked { get; set; }

        public virtual Chat? Chat { get; set; }
        public virtual Profile? Profile { get; set; }
    }
}