using Common.Utils.Exceptions;
using Infraestructure.Entity.Entities.Profiles;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Profiles;

namespace TopMai.Domain.Services.Profiles.Interfaces
{
    public interface IContactService
    {
        Contact Get(Guid idContact, Guid idUser);
        object Post(Contact contact);
        object AddMultipleContactsByPhones(Guid userId, List<ContactToInvite> phoneContacts);
        object AddContactById(Guid userId, Guid contactId);
        void AddSupportChatContact(Guid id);
        void AddTopmaiPayContact(Guid id);
        List<ContactDto> GetMyContacts(Guid idUser);
        object Put(Contact newContact);
        Task<bool> Delete(Guid idContact, Guid idUser);
        Task<bool> BlockContact(Guid idContact, Guid idUser);
        Task<bool> UnblockContact(Guid idContact, Guid idUser);
    }
}
