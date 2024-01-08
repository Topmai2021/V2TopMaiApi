using Infraestructure.Entity.Entities.Chats;

namespace Infraestructure.Core.Repository.Interface
{
    public interface IMessageRepository : IRepository<Message>
    {
        IEnumerable<Message> GetAllByChat(Guid chatId, Guid userId, int page);
    }
}
