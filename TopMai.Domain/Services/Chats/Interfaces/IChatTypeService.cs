using Infraestructure.Entity.Entities.Chats;

namespace TopMai.Domain.Services.Chats.Interfaces
{
    public interface IChatTypeService
    {
        List<ChatType> GetAll();
        ChatType Get(int id);

        Task<ChatType> Post(ChatType chatType);
        Task<ChatType> Put(ChatType newChatType);
        Task<bool> Delete(int id);
    }
}
