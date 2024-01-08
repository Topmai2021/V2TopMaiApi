using Common.Utils.Enums;
using Common.Utils.Exceptions;
using Common.Utils.Helpers;
using Common.Utils.Resources;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Response.ChatResponses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using TopMai.Domain.DTO.Chats;
using TopMai.Domain.DTO.Profiles;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Domain.Services.Profiles.Interfaces;

namespace TopMai.Domain.Services.Chats
{
    public class ChatService : IChatService
    {
        #region Attributes
        private DataContext _dBContext;
        private readonly IMessageService _messageService;
        private readonly IProfileService _profileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IImageService _imageService;
        #endregion

        #region Builder
        public ChatService(
            DataContext dataContext,
            IMessageService messageService,
            IProfileService profileService,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IImageService imageService
            )
        {
            _dBContext = dataContext;
            _messageService = messageService;
            _profileService = profileService;
            _unitOfWork = unitOfWork;
            _config = configuration;
            _imageService = imageService;
        }
        #endregion

        #region Methods
        public List<Chat> GetAll() => _unitOfWork.ChatRepository.GetAll().OrderByDescending(x => x.Id).ToList();

        public Chat Get(Guid id) => _unitOfWork.ChatRepository.FirstOrDefault(c => c.Id == id);

        public async Task<Chat> Post(Chat chat)
        {
            if (chat.IdProfileSender == chat.IdProfileReceiver)
                throw new BusinessException("No puedes iniciar un chat contigo mismo");

            Chat repeated = _unitOfWork.ChatRepository.FirstOrDefault(c => (c.IdProfileSender == chat.IdProfileSender || c.IdProfileSender == chat.IdProfileReceiver)
                                                                        && (c.IdProfileReceiver == chat.IdProfileReceiver || c.IdProfileReceiver == chat.IdProfileSender));
            if (repeated != null)
                return repeated;

            if (chat.PublicationId != null)
            {
                Publication publication = _unitOfWork.PublicationRepository.FirstOrDefault(p => p.Id == chat.PublicationId
                                                                                             && p.Deleted == false);
                if (publication == null)
                    throw new BusinessException("La publicación no existe");

                if (publication.PublisherId != chat.IdProfileSender && publication.PublisherId != chat.IdProfileReceiver)
                    throw new BusinessException("La publicacion no es válida");
            }

            chat.Id = Guid.NewGuid();
            _unitOfWork.ChatRepository.Insert(chat);
            await _unitOfWork.Save();

            return chat;
        }

        public async Task<bool> Delete(Guid id)
        {
            Chat chat = _unitOfWork.ChatRepository.FirstOrDefault(c => c.Id == id);
            if (chat == null)
                throw new BusinessException("El id ingresado no es válido");

            _unitOfWork.ChatRepository.Delete(chat);
            return await _unitOfWork.Save() > 0;
        }

        public bool HasMoreMessages(Guid id, Guid userId, int? page, int? quantity)
        {
            if (quantity == null)
                quantity = 10;
            if (page == null || page == 0)
                return false;
            var count = _dBContext.MessageConfigurations.Include("Message")
                                                       .Count(x => x.ProfileId == userId
                                                                && x.Message.Deleted != true
                                                                && x.Message.ChatId == id
                                                                && x.MessageDeleted != true);

            return ((page * quantity) < count);
        }

        public async Task<Profile> ConnectToOneSignal(Guid id, string connectionId)
        {
            Profile profile = _profileService.GetProfile(id);
            if (profile == null)
                throw new BusinessException("El perfil no es válido");

            profile.OneSignalConnectionId = connectionId;
            _unitOfWork.ProfileRepository.Update(profile);
            await _unitOfWork.Save();

            return profile;
        }

        public async Task<Profile> DisconnectOneSignal(Guid id)
        {
            Profile profile = _profileService.GetProfile(id);
            if (profile == null)
                throw new BusinessException("El perfil no es válido");

            profile.OneSignalConnectionId = null;
            _unitOfWork.ProfileRepository.Update(profile);
            await _unitOfWork.Save();

            return profile;
        }

        public ChatVerifyResponse VerifyChat(VerifyChatDto chat)
        {
            Chat repeated;
            if (chat.PublicationId == null)
            {
                repeated = _unitOfWork.ChatRepository.FirstOrDefault(c => (c.IdProfileSender == chat.IdProfileSender || c.IdProfileSender == chat.IdProfileReceiver)
                                                                       && (c.IdProfileReceiver == chat.IdProfileReceiver || c.IdProfileReceiver == chat.IdProfileSender));
            }
            else
            {

                repeated = _unitOfWork.ChatRepository.FirstOrDefault(c => (c.IdProfileSender == chat.IdProfileSender || c.IdProfileSender == chat.IdProfileReceiver)
                                                                       && (c.IdProfileReceiver == chat.IdProfileReceiver || c.IdProfileReceiver == chat.IdProfileSender)
                                                                       && c.PublicationId == chat.PublicationId);
            }

            bool verified; 
            if (repeated != null) 
                verified = true; 
            else 
                verified = false;

            return new ChatVerifyResponse() { Id = repeated.Id, Verified = verified};
        }

        public async Task<List<ChatDto>> GetUserChats(Guid id, int page)
        {
            Chat supportChat = _unitOfWork.ChatRepository.FirstOrDefault(x => x.ChatTypeId == (int)Enums.ChatType.Soporte && x.IdProfileReceiver == id,
                                                                        m=>m.Messages,
                                                                        p=>p.IdProfileReceiverNavigation);
            if (supportChat == null)
                await CreateSupportChat(id);

            //create topmaiPay chat if not exist
            Chat topmaiPayChat = _unitOfWork.ChatRepository.FirstOrDefault(x => x.ChatTypeId == (int)Enums.ChatType.TopmaiPay && x.IdProfileReceiver == id);
            if (topmaiPayChat == null)
                await CreateTopmaiPayChat(id);

            // get chats
            List<Chat> chats = _unitOfWork.ChatRepository.GetAllChats(profileSender: id, page).ToList();

            List<ChatDto> chatWithMessages = new List<ChatDto>();
            if (chats.Any())
            {
                chats.ForEach(async x =>
                {
                    ChatDto result = await GetIndividualUserChat(x, id);
                    if (result != null)
                        chatWithMessages.Add(result);
                });

                chatWithMessages = chatWithMessages.OrderByDescending(x => x?.LastMessage?.DateHour).ToList();
            }

            return chatWithMessages;
        }

        private async Task<bool> CreateTopmaiPayChat(Guid idProfile)
        {
            Profile profile = _unitOfWork.ProfileRepository.FirstOrDefault(p => p.Id == idProfile);
            if (profile == null)
                throw new BusinessException("El usuario debe completar el perfil");

            string profileName = _config.GetSection("Profiles").GetSection("Admin").GetSection("Name").Value;
            string profileLastName = _config.GetSection("Profiles").GetSection("Admin").GetSection("LastName").Value;
            var profileAdmin = _profileService.GetProfile(profileLastName, profileName);

            var topmaiPayChat = new Chat()
            {
                Id = Guid.NewGuid(),
                ChatTypeId = (int)Enums.ChatType.TopmaiPay,
                IdProfileSender = profileAdmin.Id,
                IdProfileReceiver = profile.Id,
            };

            _unitOfWork.ChatRepository.Insert(topmaiPayChat);
            await _unitOfWork.Save();

            AddMessageDto message = new AddMessageDto()
            {
                ChatId = topmaiPayChat.Id,
                FromId = profile.Id,
                Content = GeneralMessages.MessagePayChat,
            };

            return await _messageService.Post(message);
        }

        private async Task CreateSupportChat(Guid idProfile)
        {
            Profile profile = _unitOfWork.ProfileRepository.FirstOrDefault(p => p.Id == idProfile);
            if (profile == null)
                throw new BusinessException("El usuario debe completar el perfil");

            string profileName = _config.GetSection("Profiles").GetSection("Support").GetSection("Name").Value;
            string profileLastName = _config.GetSection("Profiles").GetSection("Support").GetSection("LastName").Value;
            var profileSupport = _profileService.GetProfile(profileLastName, profileName);

            var supportChat = new Chat()
            {
                Id = Guid.NewGuid(),
                ChatTypeId = (int)Enums.ChatType.Soporte,
                IdProfileSender = profileSupport.Id,
                IdProfileReceiver = profile.Id,
            };

            _unitOfWork.ChatRepository.Insert(supportChat);
            await _unitOfWork.Save();

            AddMessageDto message = new AddMessageDto()
            {
                ChatId = supportChat.Id,
                FromId = profile.Id,
                Content = GeneralMessages.MessageSupport,
            };

            await _messageService.Post(message);
        }

        private async Task<ChatDto> GetIndividualUserChat(Chat chat, Guid id)
        {
            ChatDto chatResult = new ChatDto();
            var chatConfiguration = chat.ChatConfigurations.FirstOrDefault();

            if (chatConfiguration == null)
            {
                ChatConfiguration newChatConfiguration = new ChatConfiguration()
                {
                    Id = Guid.NewGuid(),
                    ChatId = chat.Id,
                    ProfileId = id,
                    ChatDeleted = false,
                    CreatedAt = DateTime.Now,
                    Silenced = false,
                    AlwaysUp = false,
                    Deleted = false,
                    Blocked = false,
                };

                _unitOfWork.ChatConfigurationRepository.Insert(newChatConfiguration);
                bool re = await _unitOfWork.Save() > 0;

                chatResult.ChatConfiguration = new ChatConfigurationDto()
                {
                    AlwaysUp = false,
                    ChatId = chat.Id,
                    ProfileId = id,
                    Silenced = false,
                    Blocked = false
                };
            }
            else
            {
                chatResult.ChatConfiguration = new ChatConfigurationDto()
                {
                    AlwaysUp = chatConfiguration.AlwaysUp == true,
                    ChatId = chat.Id,
                    ProfileId = id,
                    Silenced = chatConfiguration.Silenced == true,
                    Blocked = chatConfiguration.Blocked == true,
                    BackgroundUrl = chatConfiguration.BackgroundUrl,
                };
            }

            var last = chat.Messages.FirstOrDefault();
            if (last != null)
            {
                chatResult.NewMessagesAmount = chat.Messages.Count(m => !m.Readed && m.FromId != id);
                chatResult.LastMessage = new ConsultMessageDto()
                {
                    ChatId = last.ChatId,
                    Content = Helper.TextBase64Decrypt(last.Content),
                    DateHour = last.DateHour,
                    FromId = last.FromId,
                    Id = last.Id,
                    MessageTypeId = last.MessageTypeId,
                    Readed = last.Readed
                };
            }

            chatResult.ProfileSender = new ConsultProfileDto()
            {
                Id = chat.IdProfileSenderNavigation.Id,
                City = chat.IdProfileSenderNavigation.City,
                Description = chat.IdProfileSenderNavigation.Description,
                Land = chat.IdProfileSenderNavigation.Land,
                Name = chat.IdProfileSenderNavigation.Name,
                LastName = chat.IdProfileSenderNavigation.LastName,
                Latitude = chat.IdProfileSenderNavigation.Latitude,
                Lenguages = chat.IdProfileSenderNavigation.Lenguages,
                Longitude = chat.IdProfileSenderNavigation.Longitude,
                Phone = chat.IdProfileSenderNavigation.Phone,
                PostalCode = chat.IdProfileSenderNavigation.PostalCode,
                ProfileUrl = chat.IdProfileSenderNavigation.ProfileUrl,
                Sales = chat.IdProfileSenderNavigation.Sales,
                State = chat.IdProfileSenderNavigation.State,
                WalletId = chat.IdProfileSenderNavigation.WalletId,
                UrlImage = _imageService.GetUrlImage(chat.IdProfileSenderNavigation?.Image),
                StrGender = Helper.GetGender(chat.IdProfileSenderNavigation.GenderId),
            };
            chatResult.ProfileReceiver = new ConsultProfileDto()
            {
                Id = chat.IdProfileReceiverNavigation.Id,
                City = chat.IdProfileReceiverNavigation.City,
                Description = chat.IdProfileReceiverNavigation.Description,
                Land = chat.IdProfileReceiverNavigation.Land,
                Name = chat.IdProfileReceiverNavigation.Name,
                LastName = chat.IdProfileReceiverNavigation.LastName,
                Latitude = chat.IdProfileReceiverNavigation.Latitude,
                Lenguages = chat.IdProfileReceiverNavigation.Lenguages,
                Longitude = chat.IdProfileReceiverNavigation.Longitude,
                Phone = chat.IdProfileReceiverNavigation.Phone,
                PostalCode = chat.IdProfileReceiverNavigation.PostalCode,
                ProfileUrl = chat.IdProfileReceiverNavigation.ProfileUrl,
                Sales = chat.IdProfileReceiverNavigation.Sales,
                State = chat.IdProfileReceiverNavigation.State,
                WalletId = chat.IdProfileReceiverNavigation.WalletId,
                UrlImage = _imageService.GetUrlImage(chat.IdProfileReceiverNavigation?.Image),
                StrGender = Helper.GetGender(chat.IdProfileReceiverNavigation.GenderId),
            };

            return chatResult;
        }


        public Chat GetChatId(Guid userIdOne, Guid userIdTwo, Guid? publicationId)
        {
            Chat chat = _dBContext.Chats.FirstOrDefault
                (c => ((c.IdProfileSender == userIdOne && c.IdProfileReceiver == userIdTwo)
                || (c.IdProfileReceiver == userIdOne && c.IdProfileSender == userIdTwo)));
            if (chat != null)
            {
                return chat;
            }
            else
            {
                throw new BusinessException("No existe el chat");
            }
        }

        public Message GetLastMessage(Guid id, Guid userId)
        {
            Message message = _dBContext.Messages.Include("MessageConfigurations").Where(m => m.ChatId == id && m.Deleted != true)
                                                 .OrderByDescending(m => m.DateHour).FirstOrDefault();


            if (message != null)
                message.MessageConfiguration = message.MessageConfigurations.FirstOrDefault(m => m.ProfileId == userId && m.MessageDeleted != true);

            return message;
        }

        #endregion
    }
}
