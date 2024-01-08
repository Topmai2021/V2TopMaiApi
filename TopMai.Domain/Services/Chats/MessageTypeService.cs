using Common.Utils.Exceptions;
using Common.Utils.Helpers;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Chats;
using System.Reflection;
using System.Text.RegularExpressions;
using TopMai.Domain.Services.Chats.Interfaces;

namespace TopMai.Domain.Services.Chats
{
    public class MessageTypeService : IMessageTypeService
    {
        #region Atrributes
        private readonly IUnitOfWork _unitOfWork;
        #endregion
        #region Builder
        public MessageTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        #endregion

        #region Methods
        public List<MessageType> GetAll() => _unitOfWork.MessageTypeRepository.GetAll().OrderByDescending(x => x.Id).ToList();

        public MessageType Get(int id) => _unitOfWork.MessageTypeRepository.FirstOrDefault(u => u.Id == id);

        public async Task<MessageType> Post(MessageType messageType)
        {
            Helper.ValidateName(messageType.Name);

            if (NameIsRepeated(messageType.Name))
                throw new BusinessException("El tipo de mensaje ya existe en el sistema");

            messageType.Id = IdMessageType();
            _unitOfWork.MessageTypeRepository.Insert(messageType);
            await _unitOfWork.Save();

            return messageType;
        }

        public async Task<MessageType> Put(MessageType newMessageType)
        {
            MessageType messageType = Get(newMessageType.Id);
            if (messageType == null)
                throw new BusinessException("El id no coincide con ningun tipo de mensaje  ");

            messageType.Name = newMessageType.Name;
            messageType.Deleted = newMessageType.Deleted;

            _unitOfWork.MessageTypeRepository.Update(messageType);
            await _unitOfWork.Save();

            return messageType;
        }

        public async Task<bool> Delete(int id)
        {
            MessageType messageType = Get(id);
            if (messageType == null)
                throw new BusinessException("El id no coincide con ningun tipo de mensaje  ");

            messageType.Deleted = true;
            _unitOfWork.MessageTypeRepository.Update(messageType);

            return await _unitOfWork.Save() > 0;
        }

        private bool NameIsRepeated(string name)
        {
            var repeatName = (MessageType?)_unitOfWork.MessageTypeRepository.FirstOrDefault(m => m.Name == name);
            if (repeatName != null)
                return true;

            return false;
        }

        private int IdMessageType()
        {
            int id = 0;
            try
            {
                id = _unitOfWork.MessageTypeRepository.GetAll().Max(x => x.Id) + 1;
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
