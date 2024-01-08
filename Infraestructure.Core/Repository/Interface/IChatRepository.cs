using Infraestructure.Entity.Entities.Chats;

namespace Infraestructure.Core.Repository.Interface
{
    public interface IChatRepository : IRepository<Chat>
    {
        IEnumerable<Chat> GetAllChats(Guid profileSender, int page);
    }
}
