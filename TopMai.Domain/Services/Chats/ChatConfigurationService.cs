using Common.Utils.Exceptions;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Entities.Profiles;
using log4net.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.DTO.Chats;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Domain.Services.Profiles.Interfaces;

namespace TopMai.Domain.Services.Chats
{
    public class ChatConfigurationService : IChatConfigurationService
    {
        #region Attributes
        private DataContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Builder
        public ChatConfigurationService(DataContext dBContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dBContext;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods

        public ChatConfiguration Get(ChatConfiguration chatConfiguration)
        {
            ChatConfiguration confi = _unitOfWork.ChatConfigurationRepository.FirstOrDefault(x => x.ProfileId == chatConfiguration.ProfileId
                                                                                               && x.ChatId == chatConfiguration.ChatId);
            return confi;
        }

        public async Task<bool> Put(ChatConfigurationDto chatConfig)
        {
            ChatConfiguration chatConfiguration = _unitOfWork.ChatConfigurationRepository.FirstOrDefault(x => x.ChatId == chatConfig.ChatId
                                                                                                           && x.ProfileId == chatConfig.ProfileId);

            if (chatConfiguration == null)
                throw new BusinessException("No se encontró una configuración del chat");

            chatConfiguration.Blocked = chatConfig.Blocked;
            chatConfiguration.Silenced = chatConfig.Silenced;
            chatConfiguration.AlwaysUp = chatConfig.AlwaysUp;
            chatConfiguration.BackgroundUrl = chatConfig.BackgroundUrl;
            chatConfiguration.UpdatedAt = DateTime.Now;

            _unitOfWork.ChatConfigurationRepository.Update(chatConfiguration);

            return  await _unitOfWork.Save() > 0;
        }

        public object Delete(Guid id)
        {
            ChatConfiguration chatConfiguration = _dbContext.ChatConfigurations.FirstOrDefault(u => u.Id == id);
            if (chatConfiguration == null) return new { error = "El id ingresado no pertenece a ningun tipo de chat" };
            chatConfiguration.Deleted = true;
            _dbContext.Entry(chatConfiguration).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return chatConfiguration;
        }

        public object ClearChat(ChatConfiguration chatConfiguration)
        {
            ChatConfiguration chatConfigurationDB = _dbContext.ChatConfigurations.FirstOrDefault(c => c.ChatId == chatConfiguration.ChatId && c.ProfileId == chatConfiguration.ProfileId);
            if (chatConfigurationDB == null)
                return new { error = "El chat no existe" };
            Chat chat = _dbContext.Chats.FirstOrDefault(c => c.Id == chatConfiguration.ChatId);
            if (chat == null)
                return new { error = "El chat no existe" };
            List<Message> messages = _dbContext.Messages.Where(m => m.ChatId == chat.Id).ToList();
            foreach (Message message in messages)
            {
                MessageConfiguration messageConfiguration = _dbContext.MessageConfigurations.FirstOrDefault(m => m.MessageId == message.Id
                                                                                                              && m.ProfileId == chatConfiguration.ProfileId);
                if (messageConfiguration == null)
                {
                    messageConfiguration = new MessageConfiguration();
                    messageConfiguration.MessageId = message.Id;
                    messageConfiguration.ProfileId = chatConfiguration.ProfileId;
                    messageConfiguration.MessageDeleted = true;
                    messageConfiguration.Deleted = false;
                    messageConfiguration.CreatedAt = DateTime.Now;
                    messageConfiguration.UpdatedAt = DateTime.Now;
                    _dbContext.MessageConfigurations.Add(messageConfiguration);
                    _dbContext.SaveChanges();
                }
                else
                {
                    messageConfiguration.MessageDeleted = true;
                    messageConfiguration.Deleted = false;
                    messageConfiguration.UpdatedAt = DateTime.Now;
                    _dbContext.Entry(messageConfiguration).State = EntityState.Modified;
                    _dbContext.SaveChanges();

                }
            }

            return chatConfigurationDB;
        }


        #endregion
    }
}