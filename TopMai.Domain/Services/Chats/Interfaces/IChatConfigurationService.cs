using Infraestructure.Entity.Entities.Chats;
using TopMai.Domain.DTO.Chats;

namespace TopMai.Domain.Services.Chats.Interfaces
{
    public interface IChatConfigurationService
    {

        ChatConfiguration Get(ChatConfiguration chatConfiguration);
        Task<bool> Put(ChatConfigurationDto chatConfig);
        object Delete(Guid id);
        object ClearChat(ChatConfiguration chatConfiguration);
    }
}
