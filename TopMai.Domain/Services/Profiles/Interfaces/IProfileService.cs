using Infraestructure.Entity.Entities.Profiles;
using TopMai.Domain.DTO.Profiles;

namespace TopMai.Domain.Services.Profiles.Interfaces
{
    public interface IProfileService
    {
        List<Profile> GetAll();
        Profile GetProfile(string lastName, string name);
        Profile GetProfile(Guid id);
        Task<ConsultProfileDto> Get(Guid idProfile);
        Task<Profile> Post(AddProfile_Dto profile);
        Task<bool> Put(Profile newProfile);
        object SendFriendRequest(Guid fromId, Guid toId);
        object GetAllFriendRequest(Guid id);
        object GetAllFriendsToInvite(Guid id);
        object GetAllFriends(Guid id);
        object GetSellerLevelByProfileId(Guid id);
        object AcceptFriendRequest(Guid id);
        object GetCart(Guid id);
        object AddToCart(Guid id, Guid publicationId, int amount);
        object DeleteCartPublication(Guid id);
        object EditCartPublication(Guid id, int amount);
        object Search(string query, Guid userId);
    
        object SearchContacts(string query,Guid userId);
        object Delete(Guid id);

    }
}
