namespace Infraestructure.Entity.Entities.Chats
{
    public partial class ChatType
    {
        public ChatType()
        {
            Chats = new HashSet<Chat>();
        }

        public string Name { get; set; } = null!;
        public bool Deleted { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Chat> Chats { get; set; }
    }
}