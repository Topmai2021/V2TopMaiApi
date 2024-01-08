using Common.Utils.Enums;
using Common.Utils.Exceptions;
using Common.Utils.Helpers;
using Common.Utils.Resources;
using Infraestructure.Core.Dapper;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Chats;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt;
using OneSignal.RestAPIv3.Client;
using OneSignal.RestAPIv3.Client.Resources;
using OneSignal.RestAPIv3.Client.Resources.Notifications;
using TopMai.Domain.DTO.Chats;
using TopMai.Domain.DTO.Profiles;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Domain.Services.Profiles.Interfaces;

namespace TopMai.Domain.Services.Chats
{
    public class MessageService : IMessageService
    {
        #region Atrributes
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IImageService _imageService;
        #endregion

        #region Builder
        public MessageService(IUnitOfWork unitOfWork, IConfiguration configuration, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _config = configuration;
            _imageService = imageService;
        }
        #endregion

        #region Methods

        public List<ConsultMessageDto> GetAllByChat(Guid chatId, Guid userId, int page)
        {
            IEnumerable<Message> messages = _unitOfWork.MessageRepository.GetAllByChat(chatId, userId, page);
            List<ConsultMessageDto> list = messages.Select(x => new ConsultMessageDto()
            {
                Id = x.Id,
                ChatId = x.ChatId,
                Content = Helper.TextBase64Decrypt(x.Content),
                DateHour = x.DateHour,
                FromId = x.FromId,
                MessageTypeId = x.MessageTypeId,
                Readed = x.Readed,
                From = new ConsultProfileDto()
                {
                    Id = x.From.Id,
                    City = x.From.City,
                    Description = x.From.Description,
                    Land = x.From.Land,
                    Name = x.From.Name,
                    LastName = x.From.LastName,
                    Latitude = x.From.Latitude,
                    Lenguages = x.From.Lenguages,
                    Longitude = x.From.Longitude,
                    Phone = x.From.Phone,
                    PostalCode = x.From.PostalCode,
                    ProfileUrl = x.From.ProfileUrl,
                    Sales = x.From.Sales,
                    State = x.From.State,
                    WalletId = x.From.WalletId,
                    UrlImage = _imageService.GetUrlImage(x.From?.Image),
                    StrGender = Helper.GetGender(x.From.GenderId),
                }
            }).ToList();


            var mmessageNotReaded = messages.Where(x => !x.Readed).ToList();
            Task.Run(() => UpdateMessageReaded(mmessageNotReaded));

            var listOrdenable = list.OrderBy(x => x.DateHour);

            return listOrdenable.ToList();
        }

        //se ejecuta en sub proceso (segundo plano).
        private void UpdateMessageReaded(List<Message> messages)
        {
            string connection = _config.GetConnectionString("DefaultConnection");
            DapperHelper.Instancia.ConnectionString = connection;
            foreach (var item in messages)
            {
                var filter = new
                {
                    readed = true,
                    idMessage = item.Id,
                };
                DapperHelper.Instancia.ExecuteQueryScalar(StatementSql.updateMeesageReaded, filter);
            }
        }

        public void NotifyUser(Message message, string oneSignalId)
        {
            var strClient = _config.GetSection("ConfigOneSignal").GetSection("Client").Value;
            var strAppId = _config.GetSection("ConfigOneSignal").GetSection("AppId").Value;
            var strIcon = _config.GetSection("ConfigOneSignal").GetSection("SmallAndroidIcon").Value;

            var client = new OneSignalClient(strClient); // Use your Api Key
            var options = new NotificationCreateOptions
            {
                AppId = new Guid(strAppId),   // Use your AppId
                IncludePlayerIds = new List<string>()
                {
                    oneSignalId.ToString()
                }
            };
            options.Headings.Add(LanguageCodes.English, $"{message.FullName}");
            options.Contents.Add(LanguageCodes.English, Helper.TextBase64Decrypt(message.Content).ToString());
            //options.Data.Add("chatId", message.Chat.Id.ToString());

            options.SmallAndroidIcon = strIcon;
            options.Data = new Dictionary<string, string>();
            options.Data["chatId"] = message.Chat.Id.ToString();
            options.Data["profileId"] = message.FromId.ToString();

            //options.Url = "https://localhost:8100/"+message.ChatId.ToString()+"/"+profileId.ToString();
            client.Notifications.Create(options);

        }

        public async Task<bool> Post(AddMessageDto message)
        {
            bool isGif = false;
            Message newMeessage = new Message()
            {
                Id = Guid.NewGuid(),
                DateHour = DateTime.Now,
                FromId = message.FromId,
                ChatId = message.ChatId,
                Content = EncryptProvider.Base64Encrypt(message.Content),
                MessageTypeId = message.MessageTypeId,
            };

            ValidateMessage(message);

            if (message.Content[message.Content.Length - 1] == 'f' || message.Content[message.Content.Length - 1] == 'F')
                isGif = true;

            if (message.MessageTypeId == 0)
                message.MessageTypeId = (int)Enums.MessageType.Normal;

            //newMeessage.From = _profileService.GetProfile(message.FromId);
            var from = _unitOfWork.ProfileRepository.FirstOrDefault(x => x.Id == message.FromId);
            newMeessage.FullName = from.FullName;
            if (from == null)
                throw new BusinessException("El id del emisor no es valido");

            var chat = _unitOfWork.ChatRepository.FirstOrDefault(c => c.Id == message.ChatId);
            if (chat == null)
                throw new BusinessException("El id del chat no es valido");

            ChatConfiguration chatConfi = _unitOfWork.ChatConfigurationRepository.FirstOrDefault(c => c.ChatId == message.ChatId
                                                                                                   && c.Blocked == true);
            if (chatConfi != null)
                throw new BusinessException("Este mensaje no puede ser enviado ( usuario bloqueado )");

            if (chat.IdProfileSender != message.FromId && chat.IdProfileReceiver != message.FromId)
                throw new BusinessException("El usuario no pertenece al chat");


            //chat.ProfileOne = _unitOfWork.ProfileRepository.FirstOrDefault(x => x.Id == chat.IdProfileSender);
            //chat.ProfileTwo = _unitOfWork.ProfileRepository.FirstOrDefault(x => x.Id == chat.IdProfileReceiver);

            var oneSignalId = string.Empty;
            var profileId = string.Empty;
            //if (x.From.Id == message.FromId)
            //{
            //    oneSignalId = chat.IdProfileReceiverNavigation.OneSignalConnectionId;
            //    profileId = chat.IdProfileReceiverNavigation.Id.ToString();
            //}
            //else
            //{
            //    oneSignalId = x.From.OneSignalConnectionId;
            //    profileId = x.From.Id.ToString();
            //}

            _unitOfWork.MessageRepository.Insert(newMeessage);
            bool result = await _unitOfWork.Save() > 0;

            if (result)
            {
                //notify user
                Message messageNotify = new Message();
                messageNotify.Id = newMeessage.Id;
                messageNotify.Content = newMeessage.Content;
                messageNotify.DateHour = newMeessage.DateHour;
                messageNotify.From = from;
                messageNotify.Chat = chat;
                messageNotify.MessageType = newMeessage.MessageType;
                messageNotify.MessageTypeId = message.MessageTypeId;
                messageNotify.FromId = newMeessage.FromId;
                messageNotify.ChatId = newMeessage.ChatId;
                messageNotify.FullName = newMeessage.FullName;

                if (messageNotify.MessageTypeId == (int)Enums.MessageType.Emoticon)
                    messageNotify.Content = EncryptProvider.Base64Encrypt("💬emoticon");
                if (messageNotify.MessageTypeId == (int)Enums.MessageType.Imagen)
                {
                    if (isGif)
                        messageNotify.Content = EncryptProvider.Base64Encrypt("📼​gif");
                    else
                        messageNotify.Content = EncryptProvider.Base64Encrypt("📷imagen");
                }
                if (messageNotify.MessageTypeId == (int)Enums.MessageType.Video)
                    messageNotify.Content = EncryptProvider.Base64Encrypt("📹video");
                if (messageNotify.MessageTypeId == (int)Enums.MessageType.Audio)
                    messageNotify.Content = EncryptProvider.Base64Encrypt("🎧audio");
                if (messageNotify.MessageTypeId == (int)Enums.MessageType.Localizacion)
                    messageNotify.Content = EncryptProvider.Base64Encrypt("📍ubicación");
                if (messageNotify.MessageTypeId == (int)Enums.MessageType.Contacto)
                    messageNotify.Content = EncryptProvider.Base64Encrypt("📱contacto");
                if (messageNotify.MessageTypeId == (int)Enums.MessageType.Venta)
                    messageNotify.Content = EncryptProvider.Base64Encrypt("💬te escribió por una publicación");
                if (messageNotify.MessageTypeId == (int)Enums.MessageType.Oferta)
                    messageNotify.Content = EncryptProvider.Base64Encrypt("💲te ha enviado una oferta por tu publicación");
                if (messageNotify.MessageTypeId == (int)Enums.MessageType.Pago)
                    messageNotify.Content = EncryptProvider.Base64Encrypt("💵transferencia");

                //if (oneSignalId != null)
                //    NotifyUser(messageNotify, oneSignalId);
            }

            return result;
        }

        public async Task<bool> CreateTopmaiPayMessage(Guid userId, string content, int messageTypeId)
        {
            var user = _unitOfWork.ProfileRepository.FirstOrDefault(p => p.Id == userId);
            if (user == null)
                throw new BusinessException("El usuario no existe");

            Chat topmaiPayChat = _unitOfWork.ChatRepository.FirstOrDefault(c => c.ChatTypeId == (int)Enums.ChatType.TopmaiPay
                                                                                 && c.IdProfileReceiver == userId,
                                                                                 pS => pS.IdProfileSenderNavigation);

            AddMessageDto message = new AddMessageDto();
            message.Content = content;
            message.ChatId = topmaiPayChat.Id;
            message.FromId = topmaiPayChat.IdProfileSenderNavigation.Id;
            if (messageTypeId == 0)
                message.MessageTypeId = messageTypeId;
            else
                message.MessageTypeId = (int)Enums.MessageType.Normal;

            return await Post(message);
        }

        public async Task<bool> CreateSupportMessage(Guid userId, string content)
        {
            var user = _unitOfWork.ProfileRepository.FirstOrDefault(p => p.Id == userId);
            if (user == null)
                throw new BusinessException("El usuario no existe");

            Chat supportChat = _unitOfWork.ChatRepository.FirstOrDefault(c => c.ChatTypeId == (int)Enums.ChatType.Soporte
                                                                              && c.IdProfileReceiver == userId,
                                                                              pS => pS.IdProfileSenderNavigation);

            AddMessageDto message = new AddMessageDto();
            message.Content = content;
            message.ChatId = supportChat.Id;
            message.FromId = supportChat.IdProfileSenderNavigation.Id;
            message.MessageTypeId = (int)Enums.MessageType.Normal;

            return await Post(message);
        }

        public async Task<bool> Delete(Guid messageId)
        {
            Message message = _unitOfWork.MessageRepository.FirstOrDefault(p => p.Id == messageId);
            if (message == null)
                throw new BusinessException("El id ingresado no es válido ");

            _unitOfWork.MessageRepository.Delete(message);

            return await _unitOfWork.Save() > 0;
        }


        private void ValidateMessage(AddMessageDto message)
        {
            if (string.IsNullOrEmpty(message.Content) || message.Content.Length < 1)
                throw new BusinessException("El contenido debe tener mínimo 1 carácter.");

            if (message.FromId == null || message.FromId.ToString().Length < 7)
                throw new BusinessException("El id del emisor no es valido");
        }
        #endregion
    }
}
