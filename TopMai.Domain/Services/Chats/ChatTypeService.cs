using Common.Utils.Exceptions;
using Common.Utils.Helpers;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Chats;
using TopMai.Domain.Services.Chats.Interfaces;

namespace TopMai.Domain.Services.Chats
{
    public class ChatTypeService : IChatTypeService
    {
        #region Attributes
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Builder
        public ChatTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        #endregion

        #region Methods
        public List<ChatType> GetAll() => _unitOfWork.ChatTypeRepository.FindAll(x => !x.Deleted).ToList();

        public ChatType Get(int id) => _unitOfWork.ChatTypeRepository.FirstOrDefault(c => c.Id == id && !c.Deleted);

        public async Task<ChatType> Post(ChatType chatType)
        {
            Helper.ValidateName(chatType.Name);

            if (NameIsRepeated(chatType.Name))
                throw new BusinessException("El tipo de chat ya existe en el sistema");

            chatType.Id = IdChatType();
            chatType.Deleted = false;

            _unitOfWork.ChatTypeRepository.Insert(chatType);
            await _unitOfWork.Save();

            return chatType;
        }

        public async Task<ChatType> Put(ChatType newChatType)
        {
            ChatType chatType = Get(newChatType.Id);
            if (chatType == null)
                throw new BusinessException("El id no coincide con ningun tipo de chat");

            chatType.Name = newChatType.Name;
            chatType.Deleted = newChatType.Deleted;

            _unitOfWork.ChatTypeRepository.Update(chatType);
            await _unitOfWork.Save();

            return chatType;

        }

        public async Task<bool> Delete(int id)
        {
            ChatType chatType = Get(id);
            if (chatType == null)
                throw new BusinessException("El id no coincide con ningun tipo de chat");

            chatType.Deleted = true;
            _unitOfWork.ChatTypeRepository.Update(chatType);

            return await _unitOfWork.Save() > 0;
        }

        public bool NameIsRepeated(string name)
        {
            var repeatName = (ChatType?)_unitOfWork.ChatTypeRepository.FirstOrDefault(r => r.Name == name);
            if (repeatName != null) return true;
            return false;

        }

        private int IdChatType()
        {
            int id = 0;
            try
            {
                id = _unitOfWork.ChatTypeRepository.GetAll().Max(x => x.Id) + 1;
            }
            catch (Exception)
            {
                id = 1;
            }

            return id;
        }

        #endregion
    }
}
