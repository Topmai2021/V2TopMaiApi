using Common.Utils.Enums;
using Common.Utils.Exceptions;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.DTOs;
using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Users;
using Infraestructure.Entity.Request;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Domain.Services.Payments.Interfaces;


namespace TopMai.Domain.Services.Payments
{
    public class SellRequestService : ISellRequestService
    {
        #region Attributes
        private DataContext _dBContext;
        private ISellService _sellService;
        private IMessageService _messageService;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Builder
        public SellRequestService(DataContext dBContext, ISellService sellService, IMessageService messageService, IUnitOfWork unitOfWork)
        {
            _dBContext = dBContext;
            _sellService = sellService;
            _messageService = messageService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public List<SellRequest> GetAll() => _unitOfWork.SellRequestRepository.GetAll().ToList();

        public SellRequest Get(Guid id)
        {

            SellRequest? sellRequest = _unitOfWork.SellRequestRepository
                .FirstOrDefault(x => x.Id == id && !x.Deleted, 
                p => p.Publication,
                s => s.Status,
                a => a.Address,
                se => se.Sell);
                                                

            if (sellRequest == null)
                throw new BusinessException("No se encontro la venta solicitada");

            Sell sell = _unitOfWork.SellRepository.FirstOrDefault(x => x.Id == sellRequest.SellId);

            if (sellRequest.Sell != null)
            {
                sellRequest.Sell.Devolution = sell.Devolution;

            }

            return sellRequest ?? throw new BusinessException("No se encontro la venta solicitada");
        }
        public async Task<SellRequest> Post(SellRequestDTO sellRequest)
        {
            sellRequest.Id = Guid.NewGuid();
            sellRequest.CurrencyId = (int)Enums.Currency.MXN;

            //sellRequest.EndDateTime = DateTime.Now.AddDays(2);

            User user = _unitOfWork.UserRepository.FirstOrDefault(u => u.Id == sellRequest.UserId);
            if (user == null)
                throw new BusinessException("El usuario no es válido");

            Publication publication = _unitOfWork.PublicationRepository.FirstOrDefault(u => u.Id == sellRequest.PublicationId);
            if (publication == null)
                throw new BusinessException("La publicación no es válida");

            if (publication.PublisherId == user.Id)
                throw new BusinessException("No puedes ofertar tu propia publicación");

            if (publication.CurrencyId != sellRequest.CurrencyId)
                throw new BusinessException("No se puede ofertar una publicación con divisa distinta a peso mexicano");

            if (sellRequest.Total < 1)
                throw new BusinessException("El total no puede ser menor a 1");

            if (sellRequest.Total < (publication.Price * 0.60))
                throw new BusinessException("El total a ofertar no puede ser menor al 60% del precio publicado ( precio minimo a ofertar $" + publication.Price * 0.60 + " )");


            //sellRequest.TotalOffered = sellRequest.Total;
            // float commission = (float)sellRequest.Total * 0.05F + 15;
            var shippmentPrice = publication.ShippmentPrice;
            if (shippmentPrice == null)
                shippmentPrice = 0;

            if (sellRequest.WithShippment == true)
            {
                if (sellRequest.AddressId == null)
                    throw new BusinessException("No se puede ofertar con envío sin una dirección de envío");

            }

            if (publication.ReceiveOffers == false && sellRequest.Total < publication.Price)
                throw new BusinessException("El vendedor no acepta ofertas menores al precio publicado ( precio minimo a ofertar $" + publication.Price + " )");

            int countSellRequests = _dBContext.SellRequests
                                        .Where(x => x.PublicationId == sellRequest.PublicationId
                                            && x.UserId == sellRequest.UserId
                                            && x.StatusId == (int)Enums.State.Pendiente_SellRequest
                                            && x.DateTime == DateTime.Now).Count();
            if (countSellRequests > 5)
                throw new BusinessException("Solo puedes ofertar 5 veces por publicación al dia");

            if (sellRequest.Amount < 1)
                sellRequest.Amount = 1;

            //if (sellRequest.WithShippment != false)
            //{
            //    //sellRequest.TotalWithCommission = sellRequest.Total + commission + shippmentPrice;
            //    sellRequest.Total = sellRequest.Total + shippmentPrice;

            //}
            //else
            //    sellRequest.TotalWithCommission = sellRequest.Total;

            Status status = _unitOfWork.StatusRepository.FirstOrDefault(u => u.Id == sellRequest.StatusId);
            if (status == null)
                sellRequest.StatusId = (int)Enums.State.Pendiente_SellRequest;

            var newSellRequest = new SellRequest
            {
                Amount = sellRequest.Amount,
                Total = sellRequest.Total,
                PublicationId = sellRequest.PublicationId,
                MeetingPlace = sellRequest.MeetingPlace,
                MeetingTime = sellRequest.MeetingTime,
                ClothingColor = sellRequest.ClothingColor,
                WithShippment = sellRequest.WithShippment,
                AddressId = sellRequest.AddressId,
                CurrencyId = (sellRequest.CurrencyId == 0) ? (int)Enums.Currency.MXN : sellRequest.CurrencyId,
                StatusId = sellRequest.StatusId,
            };

            _unitOfWork.SellRequestRepository.Insert(newSellRequest);
            await _unitOfWork.Save();

            return newSellRequest;
        }

        public async Task<SellRequest> Put(SellRequest newSellRequest)
        {
            var idSellRequest = newSellRequest.Id;
            if (idSellRequest == null || idSellRequest.ToString().Length < 6)
                throw new BusinessException("Ingrese un id de oferta válida");



            SellRequest? sellRequest = _unitOfWork.SellRequestRepository.FirstOrDefault(r => r.Id == idSellRequest && newSellRequest.Id != null);
            if (sellRequest == null)
                throw new BusinessException("El id no coincide con ninguna solicitud");


            sellRequest.Total = newSellRequest.Total;
            sellRequest.SellId = newSellRequest.SellId;
            sellRequest.TotalOffered = newSellRequest.TotalOffered;
            sellRequest.AddressId = newSellRequest.AddressId;

            sellRequest.PublicationId = newSellRequest.PublicationId;
            sellRequest.Amount = newSellRequest.Amount;
            sellRequest.TotalWithCommission = newSellRequest.TotalWithCommission;
            sellRequest.StatusId = newSellRequest.StatusId;

            sellRequest.CurrencyId = newSellRequest.CurrencyId;
            sellRequest.DateTime = newSellRequest.DateTime;
            sellRequest.WithShippment = newSellRequest.WithShippment;
            sellRequest.DeliveryType = newSellRequest.DeliveryType;
            sellRequest.MeetingPlace = newSellRequest.MeetingPlace;
            sellRequest.MeetingTime = newSellRequest.MeetingTime;
            sellRequest.ClothingColor = newSellRequest.ClothingColor;
            sellRequest.Deleted = newSellRequest.Deleted;

            _unitOfWork.SellRequestRepository.Update(sellRequest);
            await _unitOfWork.Save();

            return sellRequest;
        }

        public object GetComissions(Guid publicationId, float total, bool? withShippment)
        {
            Publication publication = _dBContext.Publications.FirstOrDefault(u => u.Id == publicationId);
            if (publication == null)
                return new { error = "La publicación no es válida" };
            if (publication.PublisherId == null)
                return new { error = "El vendedor no existe" };
            if (total < 1)
                return new { error = "El total no puede ser menor a 1" };

            float commission;
            float shippmentPrice;
            float totalToPay;
            if (withShippment == false)
            {
                commission = 0;
                shippmentPrice = 0;
                totalToPay = total;
            }
            else
            {
                commission = total * 0.05F + 15;
                shippmentPrice = 0;
                if (publication.ShippmentPrice != null)
                    shippmentPrice = (float)publication.ShippmentPrice;


                totalToPay = total + commission + shippmentPrice;

            }

            return new { commission = commission, shippmentPrice = shippmentPrice, totalToPay = totalToPay };

        }
        public object Delete(Guid id)
        {
            SellRequest sellRequest = _dBContext.SellRequests.FirstOrDefault(u => u.Id == id);
            if (sellRequest == null)
                return new { error = "El id ingresado no es válido" };

            sellRequest.Deleted = true;
            _dBContext.Entry(sellRequest).State = EntityState.Modified;
            _dBContext.SaveChanges();

            return sellRequest;
        }

        public object GetBuyOffersByUserId(Guid id)
        {
            User user = _dBContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return new { error = "El usuario no es válido" };
            List<SellRequest> sellRequests = _dBContext.SellRequests.Include("Publication")
                                                                   .Include("Status")
                                                                   .Include("Publication.Currency")
                                                                   .Include("Address")
                                                                   .Where(r => r.UserId == id
                                                                            && r.Publication.PublisherId != id
                                                                            && r.Deleted != true)
                                                                   .OrderByDescending(x => x.DateTime).ToList();

            return sellRequests;

        }
        public object GetSellOffersByUserId(Guid id)
        {
            User user = _dBContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return new { error = "El usuario no es válido" };
            List<SellRequest> sellRequests = _dBContext.SellRequests.Include("Publication")
                                                                    .Include("Status")
                                                                    .Include("Publication.Currency")
                                                                    .Include("Address")
                                                                    .Where(r => r.Publication.PublisherId == id
                                                                    && r.Deleted != true)
                                                                    .OrderByDescending(x => x.DateTime).ToList();

            return sellRequests;
        }

        public object AcceptSellOffer(Guid sellRequestId, Guid sellerId)
        {
            SellRequest sellRequest = _dBContext.SellRequests.Include("Publication")
                                                             .FirstOrDefault(r => r.Id == sellRequestId && r.Deleted != true);

            if (sellRequest == null)
                return new { error = "La oferta no es válida" };

            if (sellRequest.EndDateTime != null && sellRequest.EndDateTime < DateTime.Now)
            {
                sellRequest.StatusId = (int)Enums.State.Rechazada_SellRequest;
                _dBContext.Entry(sellRequest).State = EntityState.Modified;
                _dBContext.SaveChanges();

                return new { error = "La oferta ya expiró" };

            }

            if (sellRequest.StatusId == (int)Enums.State.Pendiente_SellRequest)
                return new { error = "La oferta ya fue resuelta" };

            if (sellRequest.Publication.PublisherId != sellerId)
                return new { error = "Usted no puede aceptar esta oferta ya que no es el publicante" };

            sellRequest.StatusId = (int)Enums.State.Aceptada_SellRequest;

            sellRequest.EndDateTime = DateTime.Now.AddDays(2);

            _dBContext.Entry(sellRequest).State = EntityState.Modified;
            _dBContext.SaveChanges();

            User buyer = _dBContext.Users.Include("Profile")
                                .FirstOrDefault(u => u.Id == sellRequest.UserId);
            if (buyer.Profile.OneSignalConnectionId != null)
            {
                Message message = new Message();
                message.From = _dBContext.Profiles.FirstOrDefault(p => p.Id == sellerId);
                message.Content = EncryptProvider.Base64Encrypt("Ha aceptado la oferta").ToString();
                message.Chat = _dBContext.Chats.FirstOrDefault(c => (c.IdProfileSender == buyer.Id && c.IdProfileReceiver == sellerId)
                                                                    || (c.IdProfileSender == sellerId && c.IdProfileReceiver == buyer.Id));

                _messageService.NotifyUser(message, buyer.Profile.OneSignalConnectionId);
            }

            return sellRequest;
        }

        public object DeclineSellOffer(Guid sellRequestId, Guid sellerId)
        {
            SellRequest sellRequest = _dBContext.SellRequests.Include("Publication")
                                                             .FirstOrDefault(r => r.Id == sellRequestId && r.Deleted != true);
            if (sellRequest == null)
                return new { error = "La oferta no es válida" };

            if (sellRequest.StatusId == (int)Enums.State.Pendiente_SellRequest)
                return new { error = "La oferta ya fue resuelta" };

            if (sellRequest.Publication.PublisherId != sellerId)
                return new { error = "Usted no puede rechazar esta oferta ya que no es el publicante" };

            sellRequest.StatusId = (int)Enums.State.Rechazada_SellRequest;

            _dBContext.Entry(sellRequest).State = EntityState.Modified;
            _dBContext.SaveChanges();

            return sellRequest;
        }

        public async Task<Sell> ConfirmSellOffer(SellOfferRequest sellOffer)
        {
            SellRequest sellRequest = _dBContext.SellRequests.Include("Publication")
                                                             .FirstOrDefault(r => r.Id == sellOffer.sellRequestId
                                                             && r.Deleted != true);
            if (sellRequest == null)
                throw new BusinessException("La oferta no es válida");

            if (sellRequest.StatusId == (int)Enums.State.Aceptada_SellRequest)
                throw new BusinessException("La oferta debe estar aceptada para confirmar");

            if (sellRequest.Publication.PublisherId == sellOffer.userId)
                throw new BusinessException("Usted no puede confirmar esta oferta ya que es el publicante");

            sellRequest.StatusId = (int)Enums.State.Confirmada_SellRequest;

            //Create sell
            Sell sell = new Sell();
            sell.Id = Guid.NewGuid();
            sell.Total = sellRequest.Total;
            sell.TotalWithCommission = sellRequest.TotalWithCommission;
            sell.TotalOffered = sellRequest.TotalOffered;
            sell.Amount = sellRequest.Amount;
            sell.PublicationId = sellRequest.PublicationId;
            sell.BuyerId = sellOffer.userId;
            sell.DateTime = DateTime.Now;
            sell.Deleted = false;
            sell.SellerId = sellRequest.Publication.PublisherId;
            if (sellRequest.WithShippment == true)
                sellOffer.paymentMethodId = (int)Enums.PaymentMethod.Normal;
            else
                sellOffer.paymentMethodId = (int)Enums.PaymentMethod.Instantaneo;

            PaymentMethod paymentMethod = _dBContext.PaymentMethods.FirstOrDefault(p => p.Id == sellOffer.paymentMethodId);

            if (paymentMethod.AccreditationDays > 4)
            {
                sell.EstimatedDeliveryDate = DateTime.Now.AddDays((double)paymentMethod.AccreditationDays - 4);
            }
            else
            {
                sell.EstimatedDeliveryDate = DateTime.Now;
            }

            var createdSell = await _sellService.Post(sell, sellOffer.paymentMethodId);

            if (createdSell.GetType() == typeof(Sell))
            {
                Sell newSell = (Sell)createdSell;
                sellRequest.SellId = newSell.Id;
                _dBContext.Entry(sellRequest).State = EntityState.Modified;
                _dBContext.SaveChanges();

                Profile seller = _dBContext.Profiles
                            .FirstOrDefault(p => p.Id == sellRequest.Publication.PublisherId);

                if (seller.OneSignalConnectionId != null)
                {
                    Message message = new Message();
                    message.From = _dBContext.Profiles.FirstOrDefault(p => p.Id == sellOffer.userId);
                    message.Content = EncryptProvider.Base64Encrypt("Ha confirmado la oferta y efectuado el pago").ToString();
                    message.Chat = _dBContext.Chats
                                        .FirstOrDefault(c => (c.IdProfileSender == seller.Id && c.IdProfileReceiver == sellOffer.userId)
                                                        || (c.IdProfileSender == sellOffer.userId && c.IdProfileReceiver == seller.Id));

                    _messageService.NotifyUser(message, seller.OneSignalConnectionId);
                }

            }

            return createdSell;
        }

        #endregion
    }
}
