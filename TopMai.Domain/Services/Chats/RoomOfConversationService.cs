using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Users;
using System.Reflection;
using TopMai.Domain.Services.Chats.Interfaces;

namespace TopMai.Domain.Services.Chats
{
    public class RoomOfConversationService : IRoomOfConversationService
    {
        #region Atrributes
        private readonly IUnitOfWork _unitOfWork;
        #endregion
        #region Builder
        public RoomOfConversationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        #endregion

        #region Methods
        public List<RoomOfConversation> GetAll() => _unitOfWork.RoomOfConversationRepository.GetAll().OrderByDescending(x => x.Id).ToList();

        public RoomOfConversation Get(Guid id) => _unitOfWork.RoomOfConversationRepository.FirstOrDefault(c => c.Id == id);

        public async Task<object> Post(RoomOfConversation roomOfConversation)
        {
            roomOfConversation.Id = Guid.NewGuid();
            roomOfConversation.Deleted = false;
            roomOfConversation.CreatedAt = DateTime.Now;
            roomOfConversation.BrokenConversation = false;

            if (roomOfConversation.UserOneId == null || roomOfConversation.UserOneId.ToString().Length < 7) return new { error = "El uIdOne debe ser válido " };
            if (roomOfConversation.UserTwoId == null || roomOfConversation.UserTwoId.ToString().Length < 7) return new { error = "El uIdTwo debe ser válido " };
            RoomOfConversation repeated = _unitOfWork.RoomOfConversationRepository
                                         .FirstOrDefault(r => (r.UserOneId == roomOfConversation.UserOneId || r.UserOneId == roomOfConversation.UserTwoId)
                                                           && (r.UserTwoId == roomOfConversation.UserOneId || r.UserTwoId == roomOfConversation.UserTwoId)
                                                           && (r.PublicationId == roomOfConversation.PublicationId));
            if (repeated != null)
            {
                repeated.UpdatedAt = DateTime.Now;
                _unitOfWork.RoomOfConversationRepository.Update(repeated);
                await _unitOfWork.Save();

                return false;
            }

            Profile profileOne = _unitOfWork.ProfileRepository.FirstOrDefault(p => p.Id == roomOfConversation.UserOneId);
            if (profileOne == null) return new { error = "El uIdOne debe tener un perfil asociado  " };
            Profile profileTwo = _unitOfWork.ProfileRepository.FirstOrDefault(p => p.Id == roomOfConversation.UserTwoId);
            if (profileTwo == null) return new { error = "El uIdTwo debe tener un perfil asociado " };
            Publication publication = _unitOfWork.PublicationRepository.FirstOrDefault(p => p.Id == roomOfConversation.PublicationId);
            if (publication == null) return new { error = "El PublicationId debe tener una publicación asociada " };

            roomOfConversation.PublicationName = publication.Name;
            roomOfConversation.PublicationImgUrl = publication.UrlPrincipalImage;

            _unitOfWork.RoomOfConversationRepository.Insert(roomOfConversation);
            await _unitOfWork.Save();

            return roomOfConversation;
        }

        public object GetRoomsByUserId(Guid userId)
        {
            User user = _unitOfWork.UserRepository.FirstOrDefault(u => u.Id == userId);
            if (user == null) return new { error = "El usuario ingresado no es válido " };

            Profile profile = _unitOfWork.ProfileRepository.FirstOrDefault(p => p.Id == userId);
            if (profile == null) return new { error = "El usuario no tiene el perfil completo " };

            return _unitOfWork.RoomOfConversationRepository.FindAll(r => (r.UserOneId == userId || r.UserTwoId == userId)).ToList(); ;
        }
        public async Task<object> Put(RoomOfConversation newRoomOfConversation)
        {
            var idRoomOfConversation = newRoomOfConversation.Id;
            if (idRoomOfConversation == null || idRoomOfConversation.ToString().Length < 6)
            {
                return new { error = "Ingrese un id de tipo de mensaje válido " };
            }

            RoomOfConversation? RoomOfConversation = _unitOfWork.RoomOfConversationRepository.FirstOrDefault(r => r.Id == idRoomOfConversation && newRoomOfConversation.Id != null);
            if (RoomOfConversation == null) return new { error = "El id no coincide con ninguna sala  " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newRoomOfConversation.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newRoomOfConversation) != null && propertyInfo.GetValue(newRoomOfConversation).ToString() != "")
                    propertyInfo.SetValue(RoomOfConversation, propertyInfo.GetValue(newRoomOfConversation));
            }

            _unitOfWork.RoomOfConversationRepository.Update(RoomOfConversation);
            await _unitOfWork.Save();

            return RoomOfConversation;
        }
        public async Task<object> Delete(Guid id)
        {
            RoomOfConversation roomOfConversation = _unitOfWork.RoomOfConversationRepository.FirstOrDefault(c => c.Id == id);
            if (roomOfConversation == null) return new { error = "El id ingresado no es válido" };

            roomOfConversation.Deleted = true;

            _unitOfWork.RoomOfConversationRepository.Update(roomOfConversation);
            await _unitOfWork.Save();

            return roomOfConversation;
        }


        #endregion
    }
}
