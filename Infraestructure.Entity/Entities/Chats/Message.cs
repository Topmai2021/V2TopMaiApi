using Infraestructure.Entity.Entities.Profiles;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Chats
{
    public partial class Message
    {
        public Message()
        {
            MessageConfigurations = new HashSet<MessageConfiguration>();
        }

        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime DateHour { get; set; }
        public Guid FromId { get; set; }
        public bool Readed { get; set; }
        public bool Deleted { get; set; }
        public Guid ChatId { get; set; }
        public int MessageTypeId { get; set; }

        public virtual Chat Chat { get; set; }
        public virtual Profile From { get; set; }
        public virtual MessageType MessageType { get; set; }
        public virtual ICollection<MessageConfiguration> MessageConfigurations { get; set; }

        [NotMapped]
        public MessageConfiguration? MessageConfiguration { get; set; }

        [NotMapped]
        public string FullName { get; set; }

    }
}