using Infraestructure.Entity.Entities.Chats;

namespace TopMai.Domain.Services.Chats.Interfaces
{
    public interface IRoomOfConversationService
    {
        List<RoomOfConversation> GetAll();

        RoomOfConversation Get(Guid id);

        Task<object> Post(RoomOfConversation roomOfConversation);
        object GetRoomsByUserId(Guid userId);
        Task<object> Put(RoomOfConversation newRoomOfConversation);
        Task<object> Delete(Guid id);
    }
}
