namespace Infraestructure.Entity.Entities.Chats
{
    public partial class MessageType
    {
        public MessageType()
        {
            Messages = new HashSet<Message>();
        }

        public string Name { get; set; } = null!;
        public bool Deleted { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}