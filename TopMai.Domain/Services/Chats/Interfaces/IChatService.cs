using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Response.ChatResponses;
using TopMai.Domain.DTO.Chats;

namespace TopMai.Domain.Services.Chats.Interfaces
{
    public interface IChatService
    {
        List<Chat> GetAll();
        Chat Get(Guid id);
        Task<Chat> Post(Chat chat);
        Task<bool> Delete(Guid id);
        bool HasMoreMessages(Guid id, Guid userId, int? page, int? quantity);
        Task<Profile> ConnectToOneSignal(Guid id, string connectionId);
        Task<Profile> DisconnectOneSignal(Guid id);
        ChatVerifyResponse VerifyChat(VerifyChatDto chat);
        Task<List<ChatDto>> GetUserChats(Guid id, int page);
        Message GetLastMessage(Guid id, Guid userId);
        Chat GetChatId(Guid userIdOne,Guid userIdTwo,Guid? publicationId);
        
    }
}
