using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Domain.Services.Chats.Interfaces;
using Common.Utils.Enums;
using Common.Utils.Exceptions;

namespace TopMai.Domain.Services.Payments
{
    public class DevolutionService : IDevolutionService
    {
        #region Attributes
        private IUnitOfWork _unitOfWork;
        private ISellService _sellService;
        private IMessageService _messageService;

        #endregion

        #region Builder
        public DevolutionService(IUnitOfWork unitOfWork, ISellService sellService, IMessageService messageService)
        {
            _unitOfWork = unitOfWork;
            _sellService = sellService;
            _messageService = messageService;


        }
        #endregion

        #region Methods
        public List<Devolution> GetAll()
        {
            List<Devolution> devolutions = _unitOfWork.DevolutionRepository
            .FindAll(x => x.Deleted != true, d => d.Status, d => d.Sell).OrderByDescending(x => x.DateTime).ToList();
            foreach (var devolution in devolutions)
            {
                devolution.Sell.Buyer = _unitOfWork.ProfileRepository.FirstOrDefault(x => x.Id == devolution.Sell.BuyerId);
                devolution.Sell.Seller = _unitOfWork.ProfileRepository.FirstOrDefault(x => x.Id == devolution.Sell.SellerId);

            }
            return devolutions;
        }

        public async Task<bool> CheckDevolutionStatus()
        {

            // if a devolution request is not approved or rejected, it will be change to pending automatically after 48 hours
            List<Devolution> devolutions = _unitOfWork.DevolutionRepository.FindAll(x => x.StatusId == (int)Enums.State.EnDisputa_Devolution
                                                                                      && x.Deleted != true).ToList();
            foreach (var devolution in devolutions)
            {
                if (devolution.DateTime != null)
                {
                    if (devolution.DateTime.Value.AddDays(2) < DateTime.Now)
                    {

                        devolution.StatusId = (int)Enums.State.Pendiente_Devolution;
                        await this.ChangeDevolutionStatus((Guid)devolution.Id, devolution.StatusId);
                    }
                }

            }

            // if a devolution request is approved, it will be change to completed automatically after 72 hours
            List<Devolution> devolutionsAceptadas = _unitOfWork.DevolutionRepository.FindAll(x => x.StatusId == (int)Enums.State.Aceptada_Devolution
                                                                                && x.Deleted != true
                                                                                && x.UpdateDate.Value.AddDays(2) < DateTime.Now
                                                                                , d => d.Sell
                                                                                ).ToList();
            foreach (var devolution in devolutionsAceptadas)
            {
                DevolutionStatusChange devolutionStatusChange = _unitOfWork.DevolutionStatusChangeRepository
                                       .FirstOrDefault(x => x.DevolutionId == devolution.Id
                                            && x.StatusId == (int)Enums.State.Aceptada_Devolution
                                            && x.Deleted != true
                                            && x.StartDate.Value.AddDays(2) < DateTime.Now
                                            && x.EndDate == null);

                if (devolutionStatusChange != null)
                {
                    Payment payment = _unitOfWork.PaymentRepository.FirstOrDefault(x => x.SellId == devolution.SellId
                                                                                && x.ToId == devolution.Sell.SellerId);

                    if (payment.StatusId == (int)Enums.State.Acreditado_Payment)
                    {
                        devolution.StatusId = (int)Enums.State.Acreditada_Devolution;

                        await ChangeDevolutionStatus((Guid)devolution.Id, devolution.StatusId);
                    }
                }

            }

            return true;
        }
        public Devolution Get(Guid id)
        {
            Devolution devolution = _unitOfWork.DevolutionRepository.FirstOrDefault(u => u.Id == id
                        , u => u.Status
                        , u => u.Sell
                        , u => u.Sell.Publication
                        );

            devolution.DevolutionStatusChanges = _unitOfWork.DevolutionStatusChangeRepository
                    .FindAll(x => x.DevolutionId == id, d => d.Status).ToList();
            devolution.CreatedBy = _unitOfWork.ProfileRepository
                    .FirstOrDefault(x => x.Id == devolution.CreatedById);
            return devolution;

        }

        public async Task<object> Post(Devolution devolution)
        {


            devolution.Id = Guid.NewGuid();
            devolution.Deleted = false;
            devolution.DateTime = DateTime.Now;
            devolution.UpdateDate = DateTime.Now;

            Sell sell = _unitOfWork.SellRepository.FirstOrDefault(u => u.Id == devolution.SellId, s => s.Publication);
            if (sell == null) return new { error = "No se encontro la venta" };

            SellRequest sellRequest = _unitOfWork.SellRequestRepository
                                    .FirstOrDefault(u => u.SellId == sell.Id);
            if (sellRequest == null) return new { error = "No se encontro la solicitud de venta" };
            if (sellRequest.WithShippment == false)
            {
                return new { error = "No se puede devolver una venta sin envio" };
            }
            Payment payment = _unitOfWork.PaymentRepository.FirstOrDefault(p => p.SellId == sell.Id);

            if (payment.ReceiptDate < DateTime.Now && payment.ReceiptDate != null)
                return new { error = "La fecha para devolver este producto ya finalizó" };

            payment.FrozenReceiptDate = payment.ReceiptDate;
            payment.ReceiptDate = DateTime.Now.AddDays(14);

            Devolution lastDevolution = _unitOfWork.DevolutionRepository
                    .FirstOrDefault(d => d.SellId == devolution.SellId);

            if (lastDevolution != null)
                return new { error = "Ya se ha realizado una solicitud de devolución para esta venta" };


            if (devolution.CreatedById != sell.BuyerId)
            {
                return new { error = "Solo el comprador puede solicitar devolución , ponganse en contacto con el para que proceda a solicitarla y posteriormente usted aceptarla" };
            }

            devolution.StatusId = (int)Enums.State.EnDisputa_Devolution;


            _unitOfWork.DevolutionRepository.Insert(devolution);


            DevolutionStatusChange devolutionStatusChange = new DevolutionStatusChange();
            devolutionStatusChange.Id = Guid.NewGuid();
            devolutionStatusChange.Deleted = false;
            devolutionStatusChange.StatusId = devolution.StatusId;
            devolutionStatusChange.DevolutionId = devolution.Id;
            devolutionStatusChange.StartDate = DateTime.Now;

            _unitOfWork.DevolutionStatusChangeRepository.Insert(devolutionStatusChange);

            await _unitOfWork.Save();


            sell.Buyer = _unitOfWork.ProfileRepository.FirstOrDefault(x => x.Id == sell.BuyerId);


            _sellService.ChangeSellStatus((Guid)sell.Id, (int)Enums.State.Devolucion_Solicitada_Sell);

            await _messageService
                 .CreateTopmaiPayMessage((Guid)sell.SellerId
                         , sell.Buyer.Name + " " + sell.Buyer.LastName + " (" + sell.Buyer.ProfileUrl +
                         ") Ha solicitado una devolución para la venta del producto '"
                         + sell.Publication.Name + "' con el número de venta #" + sell.TransactionNumber
                         + ". Por favor, revisa tu sección ventas para más detalles.", (int)Enums.MessageType.Normal);

            await _messageService
                   .CreateTopmaiPayMessage((Guid)sell.BuyerId
                           , "Has solicitado una devolución para la venta del producto '"
                           + sell.Publication.Name + "' con el número de venta #" + sell.TransactionNumber
                           + ". Por favor, revisa tu sección compras para más detalles.", (int)Enums.MessageType.Normal);

            return devolution;


        }

        public async Task<object> AcceptDevolution(Guid devolutionId, Guid userId)
        {
            Devolution devolution = _unitOfWork.DevolutionRepository.FirstOrDefault(u => u.Id == devolutionId);
            if (devolution == null)
                return new { error = "No se encontro la devolución" };

            if (devolution.StatusId != (int)Enums.State.EnDisputa_Devolution)
                return new { error = "La devolución ya no se puede aceptar" };

            devolution.StatusId = (int)Enums.State.Aceptada_Devolution;


            return await ChangeDevolutionStatus(devolutionId, devolution.StatusId);
        }

        public async Task<object> DeclineDevolution(Guid devolutionId, Guid userId)
        {
            Devolution devolution = _unitOfWork.DevolutionRepository.FirstOrDefault(u => u.Id == devolutionId);
            if (devolution == null)
                return new { error = "No se encontro la devolución" };

            if (devolution.StatusId != (int)Enums.State.EnDisputa_Devolution)
                return new { error = "La devolución ya no se puede rechazar" };

            User user = _unitOfWork.UserRepository.FirstOrDefault(u => u.Id == userId, u => u.Profile);
            if (user == null)
                return new { error = "No se encontro el usuario" };

            if (user.ProfileId != null && user.ProfileId == devolution.CreatedById)
                devolution.StatusId = (int)Enums.State.Rechazada_Devolution;
            else
                devolution.StatusId = (int)Enums.State.Pendiente_Devolution;

            return await ChangeDevolutionStatus(devolutionId, devolution.StatusId);



        }
        public async Task<object> AccreditDevolution(Guid idDevolution)
        {
            Devolution devolution = _unitOfWork.DevolutionRepository.FirstOrDefault(d => d.Id == idDevolution);
            if (devolution == null)
                return new { error = "La devolución no es válida" };

            devolution.Sell = _unitOfWork.SellRepository.FirstOrDefault(s => s.Id == devolution.SellId);
            if (devolution.Sell == null)
                return new { error = "La venta no es válida" };

            devolution.Sell.StatusId = (int)Enums.State.Devuelta_Sell;

            Payment sellPayment = _unitOfWork.PaymentRepository.FirstOrDefault(p => p.SellId == devolution.SellId
                                                                                 && p.ToId == devolution.Sell.SellerId);
            if (sellPayment == null)
                return new { error = "El pago no es válido" };

            if (sellPayment.StatusId == (int)Enums.State.Acreditado_Payment)
                return new { error = "El pago ya fue acreditado" };


            sellPayment.StatusId = (int)Enums.State.Rechazado_Payment;
            sellPayment.ReceiptDate = null;

            // generate new payment associate to the devolution
            Payment payment = new Payment();
            payment.Id = Guid.NewGuid();
            payment.TotalWithoutCommission = devolution.Sell.TotalWithCommission;

            payment.Total = devolution.Sell.TotalWithCommission;
            payment.FromId = devolution.Sell.SellerId;
            payment.ToId = devolution.Sell.BuyerId;
            payment.CurrencyId = devolution.Sell.CurrencyId;
            payment.DateHour = DateTime.Now;
            payment.ReceiptDate = DateTime.Now.AddDays(3);
            payment.SellId = devolution.SellId;
            payment.StatusId = (int)Enums.State.Pendiente_Payment;
            payment.PaymentMethodId = (int)Enums.PaymentMethod.Normal;
            _unitOfWork.PaymentRepository.Insert(payment);

            //payment.PaymentType = "Devolution";
            devolution.StatusId = (int)Enums.State.Aceptada_Devolution;


            /**
            Guid walletIdTo = (Guid)_unitOfWork.UserRepository.First(u => u.Id == devolution.Sell.BuyerId).WalletId;
            Wallet walletTo = _unitOfWork.WalletRepository.First(w => w.Id == walletIdTo);
            walletTo.Money += payment.Total;
            **/

            await _unitOfWork.Save();

            return devolution;
        }

        public async Task<object> ChangeDevolutionStatus(Guid devolutionId, int statusId)
        {
            Devolution devolution = _unitOfWork.DevolutionRepository.FirstOrDefault(u => u.Id == devolutionId
                                                                                    , d => d.Sell
                                                                                    , d => d.Sell.Publication
                                                                                    , s => s.Status);

            if (devolution == null) return new { error = "No se encontro la devolución" };

            Status status = _unitOfWork.StatusRepository.FirstOrDefault(u => u.Id == statusId);
            if (status == null)
                return new { error = "No se encontro el estado" };

            devolution.StatusId = statusId;
            devolution.Status = status;

            if (devolution.StatusId == (int)Enums.State.EnDisputa_Devolution)
            {
                return new { error = "No se puede cambiar el estado a En disputa ( es el estado inicial ) " };
            }
            
            // Validations in status changes
            if (devolution.Status.Name == "Pendiente")
            {
                DevolutionStatusChange devolutionStateChangeFinded = _unitOfWork.DevolutionStatusChangeRepository
                    .FirstOrDefault(d => d.EndDate == null
                                    && d.DevolutionId == devolutionId
                                    && d.StatusId == (int)Enums.State.EnDisputa_Devolution);
                if (devolutionStateChangeFinded == null)
                    return new { error = "Para el estado pendiente la devolución debe estar en disputa" };
            }

            
            if (devolution.StatusId== (int)Enums.State.ContraparteContactada_Devolution)
            {

                DevolutionStatusChange devolutionStatusChangeFinded = _unitOfWork.DevolutionStatusChangeRepository
                    .FirstOrDefault(d => d.EndDate == null
                                        && d.DevolutionId == devolutionId
                                        && d.StatusId == (int)Enums.State.Pendiente_Devolution);
                if (devolutionStatusChangeFinded == null)
                    return new { error = "Para el estado contraparte contactada la devolución debe estar pendiente" };

                Guid toNotify = (devolution.CreatedById == devolution.Sell.BuyerId)
                                                            ? (Guid)devolution.Sell.BuyerId
                                                            : (Guid)devolution.Sell.SellerId;

                await _messageService.CreateTopmaiPayMessage((Guid)toNotify
                         , "Actualización del estado de solicitud de devolución para el articulo '" + devolution.Sell.Publication.Name
                         + "' : La contraparte ha sido contactada y estamos aguardando respuesta.", (int)Enums.MessageType.Normal);

            }
            
            if (devolution.StatusId== (int)Enums.State.RevisandoInformacionContraparte_Devolution)
            {
                DevolutionStatusChange devolutionStatusChangeFinded = _unitOfWork.DevolutionStatusChangeRepository
                    .FirstOrDefault(d => d.EndDate == null
                                        && d.DevolutionId == devolutionId
                                        && d.StatusId == (int)Enums.State.ContraparteContactada_Devolution);

                if (devolutionStatusChangeFinded == null)
                    return new { error = "Para el estado revisando informacion contraparte la devolución tiene que tener estado contraparte contactada" };

                Guid toNotify = (devolution.CreatedById == devolution.Sell.BuyerId)
                                                            ? (Guid)devolution.Sell.BuyerId
                                                            : (Guid)devolution.Sell.SellerId;

                await _messageService.CreateTopmaiPayMessage((Guid)toNotify
                         , "Actualización del estado de solicitud de devolución para el articulo '" + devolution.Sell.Publication.Name
                         + "' : Hemos recibido respuesta de la contraparte y estamos revisando la información.", (int)Enums.MessageType.Normal);
            }

            if (devolution.Status.Name == "Aceptada")
            {
                DevolutionStatusChange devolutionStatusChangeFinded = _unitOfWork.DevolutionStatusChangeRepository
                    .FirstOrDefault(d => d.EndDate == null
                                            && d.DevolutionId == devolutionId
                                            && (d.StatusId == (int)Enums.State.RevisandoInformacionContraparte_Devolution
                                             || d.StatusId == (int)Enums.State.EnDisputa_Devolution));
                if (devolutionStatusChangeFinded == null)
                    return new { error = "Para el estado aceptado la devolución tiene que tener estado revisando información contraparte o en disputa" };

                Guid toNotify = (devolution.CreatedById == devolution.Sell.BuyerId)
                                                            ? (Guid)devolution.Sell.BuyerId
                                                            : (Guid)devolution.Sell.SellerId;

                await _messageService.CreateTopmaiPayMessage((Guid)toNotify
                         , "Actualización del estado de solicitud de devolución para el articulo '" + devolution.Sell.Publication.Name
                         + "' : La solicitud de devolución ha sido aceptada , sera acreditada en un plazo de 72 hs en caso de que la contraparte no denuncie irregularidades.", (int)Enums.MessageType.Normal);

            }

            if (devolution.Status.Name == "Rechazada")
            {
                DevolutionStatusChange devolutionStatusChangeFinded = _unitOfWork.DevolutionStatusChangeRepository
                    .FirstOrDefault(d => d.EndDate == null
                                        && d.DevolutionId == devolutionId
                                        && (d.StatusId == (int)Enums.State.Aceptada_Devolution
                                         || d.StatusId == (int)Enums.State.RevisandoInformacionContraparte_Devolution
                                         || d.StatusId == (int)Enums.State.EnDisputa_Devolution));
                if (devolutionStatusChangeFinded == null)
                    return new { error = "Para el estado rechazado la devolución tiene que tener estado aceptado o revisando información contraparte" };


                // 
                Payment payment = _unitOfWork.PaymentRepository
                    .FirstOrDefault(p => p.SellId == devolution.SellId
                                    && p.FromId == devolution.Sell.BuyerId
                                    && p.ToId == devolution.Sell.SellerId
                                    );
                if (payment == null)
                    return new { error = "No se encontro el pago" };
                // cancel devolution and continue with the sell
                payment.ReceiptDate = payment.FrozenReceiptDate;
                payment.FrozenReceiptDate = null;
                payment.StatusId = (int)Enums.State.Pendiente_Payment;


                Payment paymentDevolution = _unitOfWork.PaymentRepository
                    .FirstOrDefault(p => p.SellId == devolution.SellId
                                    && p.FromId == devolution.Sell.SellerId
                                    && p.ToId == devolution.Sell.BuyerId
                                    );
                if (paymentDevolution != null)
                {
                    paymentDevolution.ReceiptDate = null; ;
                    paymentDevolution.FrozenReceiptDate = null;
                    paymentDevolution.StatusId = (int)Enums.State.Rechazado_Payment;
                }



                _sellService.ChangeSellStatus((Guid)devolution.SellId, (int)Enums.State.DevolucionCancelada_Sell);

                Guid toNotify = (devolution.CreatedById == devolution.Sell.BuyerId)
                                                            ? (Guid)devolution.Sell.BuyerId
                                                            : (Guid)devolution.Sell.SellerId;

                await _messageService.CreateTopmaiPayMessage((Guid)toNotify
                         , "Actualización del estado de solicitud de devolución para el articulo '" + devolution.Sell.Publication.Name
                         + "' : La solicitud de devolución ha sido rechazada , el pago se ha reactivado y la venta volvera a estar en curso.", (int)Enums.MessageType.Normal);

            }

            if (devolution.Status.Name == "Acreditada")
            {

                DevolutionStatusChange devolutionStatusChangeFinded = _unitOfWork.DevolutionStatusChangeRepository
                    .FirstOrDefault(d => d.EndDate == null
                                        && d.DevolutionId == devolutionId
                                        && (d.StatusId == (int)Enums.State.Aceptada_Devolution));
                if (devolutionStatusChangeFinded == null)
                    return new { error = "Para el estado acreditado la devolución tiene que tener estado aceptado" };


                Payment sellPayment = _unitOfWork.PaymentRepository.FirstOrDefault(p => p.SellId == devolution.SellId
                                                    && p.ToId == devolution.Sell.BuyerId, p => p.Status);
                if (sellPayment == null)
                {
                    return new { error = "El pago no es válido" };
                }
                if (sellPayment.ReceiptDate > DateTime.Now)
                {
                    return new { error = "El pago aun no se ha acreditado ( fecha de acreditación : " + sellPayment.ReceiptDate + " ) " };
                }

                _sellService.ChangeSellStatus((Guid)devolution.SellId, (int)Enums.State.Devuelta_Sell);
            }




            devolution.DevolutionStatusChanges = _unitOfWork.DevolutionStatusChangeRepository
                                                 .FindAll(x => x.DevolutionId == devolutionId
                                                        && x.EndDate == null, d => d.Status).ToList();


            if (devolution.DevolutionStatusChanges.Count > 0)
            {
                for (int i = 0; i < devolution.DevolutionStatusChanges.Count; i++)
                {
                    if (devolution.DevolutionStatusChanges[i].Status.Name == "Acreditada"
                    || devolution.DevolutionStatusChanges[i].Status.Name == "Rechazada")
                    {
                        return new { error = "La devolución ya ha sido resuelta" };
                    }
                    devolution.DevolutionStatusChanges[i].EndDate = DateTime.Now;
                    _unitOfWork.DevolutionStatusChangeRepository.Update(devolution.DevolutionStatusChanges[i]);
                }

            }


            DevolutionStatusChange devolutionStatusChange = new DevolutionStatusChange();
            devolutionStatusChange.Id = Guid.NewGuid();
            devolutionStatusChange.Deleted = false;
            devolutionStatusChange.StatusId = statusId;
            devolutionStatusChange.DevolutionId = devolutionId;
            devolutionStatusChange.StartDate = DateTime.Now;



            devolution.UpdateDate = DateTime.Now;

            _unitOfWork.DevolutionStatusChangeRepository.Insert(devolutionStatusChange);

            if (devolution.StatusId == (int)Enums.State.Aceptada_Devolution)
                return await AccreditDevolution(devolutionId);

            await _unitOfWork.Save();

            return devolution;
        }
        public async Task<object> Put(Devolution newDevolution)
        {
            var idDevolution = newDevolution.Id;
            if (idDevolution == null || idDevolution.ToString().Length < 6) return new { error = "Ingrese un id de metodo de pago válido " };

            Devolution? devolution = _unitOfWork.DevolutionRepository.FirstOrDefault(r => r.Id == idDevolution && newDevolution.Id != null);
            if (devolution == null) return new { error = "El id no coincide con ningun metodo de pago " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newDevolution.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newDevolution) != null && propertyInfo.GetValue(newDevolution).ToString() != "")
                {
                    propertyInfo.SetValue(devolution, propertyInfo.GetValue(newDevolution));

                }

            }


            await _unitOfWork.Save();
            return devolution;

        }

        public object Delete(Guid id)
        {

            Devolution devolution = _unitOfWork.DevolutionRepository.FirstOrDefault(u => u.Id == id);
            if (devolution == null) return new { error = "El id ingresado no es válido" };
            devolution.Deleted = true;
            _unitOfWork.Save();
            return devolution;
        }
        #endregion

    }
}
