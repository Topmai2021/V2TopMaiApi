using Infraestructure.Entity.Entities.Chats;

namespace TopMai.Domain.Services.Chats.Interfaces
{
    public interface IMessageTypeService
    {
        List<MessageType> GetAll();
        MessageType Get(int id);
        Task<MessageType> Post(MessageType messageType);
        Task<MessageType> Put(MessageType newMessageType);
        Task<bool> Delete(int id);
    }
}
