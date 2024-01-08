using Infraestructure.Entity.Entities.Chats;
using TopMai.Domain.DTO.Chats;

namespace TopMai.Domain.Services.Chats.Interfaces
{
    public interface IMessageService
    {
        void NotifyUser(Message message, string oneSignalId);
        Task<bool> Post(AddMessageDto message);
        Task<bool> CreateSupportMessage(Guid userId,string content);
        Task<bool> CreateTopmaiPayMessage(Guid userId,string content, int messageTypeId);

        Task<bool> Delete(Guid messageId);

        List<ConsultMessageDto> GetAllByChat(Guid chatId, Guid userId, int page);
    }
}
