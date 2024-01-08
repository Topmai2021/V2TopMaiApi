using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Chats
{
    public partial class Chat
    {

        public Chat()
        {
            ChatConfigurations = new HashSet<ChatConfiguration>();
            Messages = new HashSet<Message>();
        }

        public Guid Id { get; set; }
        public Guid IdProfileSender { get; set; }
        public Guid IdProfileReceiver { get; set; }
        public string? ChatReason { get; set; }
        public Guid? PublicationId { get; set; }
        public int ChatTypeId { get; set; }

        public virtual ChatType ChatType { get; set; } = null!;
        public virtual Profile IdProfileReceiverNavigation { get; set; } = null!;
        public virtual Profile IdProfileSenderNavigation { get; set; } = null!;
        public virtual Publication? Publication { get; set; }
        public virtual ICollection<ChatConfiguration> ChatConfigurations { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}