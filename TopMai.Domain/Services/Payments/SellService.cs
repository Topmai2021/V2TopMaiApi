using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Users;
using Microsoft.EntityFrameworkCore;
using RestSharp.Extensions;
using System.Reflection;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Domain.Services.Payments.Interfaces;
using User = Infraestructure.Entity.Entities.Users.User;
using PaymentMethod = Infraestructure.Entity.Entities.Payments.PaymentMethod;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Common.Utils.Exceptions;
using Common.Utils.Enums;
using Infraestructure.Core.UnitOfWork.Interface;

namespace TopMai.Domain.Services.Payments
{
    public class SellService : ISellService
    {
        #region Attributes
        private DataContext _dBContext;
        private readonly IConfiguration _config;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IUnitOfWork _unitOfWork;
        private IMessageService _messageService;
        #endregion

        #region Builder
        public SellService(DataContext dBContext, IMessageService messageService, IConfiguration configuration, IPaymentMethodService paymentMethodService, IUnitOfWork unitOfWork)
        {
            _dBContext = dBContext;
            _messageService = messageService;
            _config = configuration;
            _paymentMethodService = paymentMethodService;
            _unitOfWork = unitOfWork;
        }
        #endregion


        #region Methods
public SellsResult GetAll(int pageNumber, int pageSize)
{
    long TotalRecords = _dBContext.Sells.Count();
    int totalPages = (int)Math.Ceiling((double)TotalRecords / pageSize);

    if (pageNumber < 1)
    {
        pageNumber = 1;
    }
    else if (pageNumber > totalPages)
    {
        pageNumber = totalPages;
    }
    int skipAmount = (pageNumber - 1) * pageSize;

    // Use a projection to select the fields you want from the "Publication" entity
    List<Sell> sells = _dBContext.Sells
        .OrderByDescending(x => x.TransactionNumber)
        .Skip(skipAmount)
        .Take(pageSize)
        .Include(s => s.Publication) // Include the "Publication" entity
        .Include(s => s.Status)
        .Select(s => new Sell
        {
            // Copy all fields from s except the "Publication.Publisher" field
            TransactionNumber = s.TransactionNumber,
            SellerId = s.SellerId,
            BuyerId = s.BuyerId,
            DateTime = s.DateTime,
            Total = s.Total,
            EstimatedDeliveryDate = s.EstimatedDeliveryDate,
            Deleted = s.Deleted,
            TotalOffered = s.TotalOffered,
            TotalWithCommission = s.TotalWithCommission,
            StatusId = s.StatusId,
            CurrencyId = s.CurrencyId,
            Amount = s.Amount,
            ShippingCode = s.ShippingCode,
            Id = s.Id,


            // Copy other fields you want to include from the "Publication" entity
            Publication = new Publication
            {
                Id = s.Publication.Id,
                Name = s.Publication.Name,
                Price = s.Publication.Price,
                Visits = s.Publication.Visits,
            },
            // Copy other fields you want to include from the "Status" entity
            Status = s.Status
        })
        .ToList();

    foreach (Sell sell in sells)
    {
        sell.Buyer = _dBContext.Profiles.Find(sell.BuyerId);
        sell.Seller = _dBContext.Profiles.Find(sell.SellerId);
    }

    var result = new SellsResult
    {
        totalPages = totalPages,
        pageNumber = pageNumber,
        TotalRecords = TotalRecords,
        sells = sells
    };

    return result;
}


        public Sell Get(Guid id)
        {
            Sell sell = _dBContext.Sells
            .Include("Status")
            .Include("Publication")
            .Include("Publication.Currency")
            .FirstOrDefault(u => u.Id == id);
            sell.Buyer = _dBContext.Profiles.Find(sell.BuyerId);
            sell.Seller = _dBContext.Profiles.Find(sell.SellerId);

            sell.StatusChanges = (List<StatusChange>)_dBContext.StatusChanges
                    .Include("Status")
                    .Where(x => x.SellId == id && x.Deleted != true).ToList();
            sell.SellRequest = (SellRequest)_dBContext.SellRequests.Where(x => x.SellId == id && x.Deleted != true).FirstOrDefault();
            if (sell.SellRequest.AddressId != null)
            {
                sell.SellRequest.Address = _dBContext.Addresses.Where(x => x.Id == sell.SellRequest.AddressId && x.Deleted != true).FirstOrDefault();
            }

            Devolution devolution = _dBContext.Devolutions
                        .Where(x => x.SellId == id && x.Deleted != true).FirstOrDefault();
            if (devolution != null)
            {
                sell.Devolution = devolution;
                sell.Devolution.DevolutionStatusChanges = (List<DevolutionStatusChange>)_dBContext.DevolutionStatusChanges.Include("Status")
                                                    .Where(x => x.DevolutionId == devolution.Id
                                                        && x.Deleted != true).OrderByDescending(d => d.StartDate).ToList();

            }

            List<Payment> payments = _dBContext.Payments
                        .Include("PaymentMethod")
                        .Include("Status")
                        .Include("From")
                        .Include("To")
                        .Where(x => x.SellId == id && x.Deleted != true).ToList();
            if (payments != null)
            {
                sell.Payments = payments;
            }

            return sell;
        }

        public async Task<Stream> CreateTracker(string? carrier, string trackingCode)
        {
            Api tracker = new Api("feimxxdt-gouo-i43w-g32u-l4hnyhxzr8vr");

            if (carrier == "" || carrier == null)
            {
                //detect carrier
                using (HttpClient client = new HttpClient())
                {
                    string urlStr = null;


                    var values = new StringContent(JsonConvert.SerializeObject(new
                    {
                        tracking_number = trackingCode

                    }));
                    values.Headers.Add("Trackingmore-Api-Key", "feimxxdt-gouo-i43w-g32u-l4hnyhxzr8vr");

                    string url = "https://api.trackingmore.com/v2/carriers/detect";
                    var rs = await client.PostAsync(url, values);
                    JObject j = JObject.Parse(rs.Content.ReadAsStringAsync().Result);
                    try
                    {
                        var st = j["data"][0].ToString();
                        urlStr = j["data"][0]["code"].ToString();
                    }
                    catch (Exception e)
                    {
                        urlStr = "";
                    }
                    carrier = urlStr;
                }
            }


            string data = "[{\"tracking_number\":\"" + trackingCode + "\",\"courier_code\":\"" + carrier + "\"}]";
            HttpResponseMessage response = tracker.doRequest("create", data, "POST");
            response.EnsureSuccessStatusCode();
            var res = response.Content.ReadAsStringAsync().Result;
            JObject json = JObject.Parse(res);
            string code = json["code"].ToString();
            string errorMessage = "";
            try
            {
                errorMessage = json["data"]["error"][0]["errorMessage"].ToString();
            }
            catch (Exception e)
            {

            }
            if (errorMessage != "" || code == "200")
            {
                return GetTracker(trackingCode);
            }
            else
            {
                return response.Content.ReadAsStreamAsync().Result;

            }

        }
        public System.IO.Stream GetTracker(string id)
        {
            Api tracker = new Api("feimxxdt-gouo-i43w-g32u-l4hnyhxzr8vr");
            string data = "?tracking_numbers=" + id;
            HttpResponseMessage response = tracker.doRequest("get" + data);
            return response.Content.ReadAsStreamAsync().Result;
        }

        public async Task<Sell> Post(Sell sell, int paymentMethodId)
        {
            sell.Id = Guid.NewGuid();
            sell.Deleted = false;

            if (sell.Total <= 0)
                throw new BusinessException("El total de la venta debe ser mayor a 0");

            Profile seller = _dBContext.Profiles.FirstOrDefault(p => p.Id == sell.SellerId
                                                                    && sell.SellerId != null);
            if (seller == null)
                throw new BusinessException("El id de vendedor no es válido");


            Profile buyer = _dBContext.Profiles.FirstOrDefault(p => p.Id == sell.BuyerId
                                                          && sell.BuyerId != null);
            if (buyer == null)
                throw new BusinessException("El id de comprador no es válido");


            Wallet buyerWallet = _dBContext.Wallets.Where(w => w.Id == buyer.WalletId).FirstOrDefault();
            if (buyerWallet == null)
                throw new BusinessException("No tiene una wallet para la divisa seleccionada");

            if (buyerWallet.Money < (sell.TotalWithCommission))
                throw new BusinessException("No tiene suficiente dinero para realizar esta compra");

            Publication publication = _dBContext.Publications.FirstOrDefault(p => p.Id == sell.PublicationId
                                        && sell.PublicationId != null);
            if (publication == null)
                throw new BusinessException("La publicación no es válida");

            if (sell.CurrencyId == 0)
                sell.CurrencyId = (int)Enums.Currency.MXN;

            if (sell.Status == null)
                sell.StatusId = (int)Enums.State.Aceptada_Sell;


            int transactionNumber;
            //generate transaction number
            if (_dBContext.Sells.Count() > 0)
                transactionNumber = (int)_dBContext.Sells.Max(x => x.TransactionNumber) + 1;
            else
                transactionNumber = 1;


            Sell repeated = _dBContext.Sells.Where(s => s.TransactionNumber == transactionNumber).FirstOrDefault();
            while (repeated != null)
            {
                transactionNumber += 1;
                repeated = _dBContext.Sells.Where(s => s.TransactionNumber == transactionNumber).FirstOrDefault();
            }

            sell.TransactionNumber = transactionNumber;

            // remove money to buyer wallet
            buyerWallet.Money -= (float)sell.TotalWithCommission;


            sell.DateTime = DateTime.Now;


            //create status change
            StatusChange statusChange = new StatusChange();
            statusChange.Id = Guid.NewGuid();
            statusChange.Deleted = false;
            statusChange.StatusId = sell.StatusId;
            statusChange.SellId = sell.Id;
            statusChange.StartDate = DateTime.Now;
            statusChange.EndDate = null;


            //generate payment
            Payment payment = new Payment();
            payment.Id = Guid.NewGuid();
            payment.Deleted = false;
            payment.TotalWithoutCommission = sell.Total;
            payment.Total = sell.TotalWithCommission;
            payment.CurrencyId = sell.CurrencyId;
            payment.DateHour = DateTime.Now;
            payment.StatusId = (int)Enums.State.Pendiente_Payment;
            PaymentMethod paymentMethod = _paymentMethodService.Get(paymentMethodId);
            if (paymentMethod == null)
                throw new BusinessException("El método de pago no es válido");

            payment.PaymentMethodId = paymentMethod.Id;
            payment.ReceiptDate = DateTime.Now + TimeSpan.FromDays((int)paymentMethod.AccreditationDays);
            payment.ToId = sell.SellerId;
            payment.FromId = sell.BuyerId;
            payment.SellId = sell.Id;

            try
            {
                _dBContext.Sells.Add(sell);
            }
            catch (Exception e)
            {
                sell.TransactionNumber += 1;
                _dBContext.Sells.Add(sell);

            }
            _dBContext.Payments.Add(payment);
            _dBContext.StatusChanges.Add(statusChange);



            if (paymentMethod.Name == "Instantaneo")
            {
                Wallet sellerWallet = _dBContext.Wallets.Where(w => w.Id == seller.WalletId).FirstOrDefault();
                sellerWallet.Money += (float)sell.Total;
                sell.StatusId = (int)Enums.State.Aceptada_Sell;
                payment.StatusId = (int)Enums.State.Acreditado_Payment;
                payment.ReceiptDate = DateTime.Now;
                _dBContext.Wallets.Update(sellerWallet);

                //notify user
                string content = JsonConvert.SerializeObject(new
                {
                    from = buyer.ProfileUrl,
                    to = seller.ProfileUrl,
                    amount = payment.TotalWithoutCommission,
                    currency = "MXN",
                    date = payment.DateHour,
                    receiptDate = payment.ReceiptDate,
                    status = "Acreditado",
                    type = "Transferencia",
                    id = payment.Id,
                    movementType = "Payment"
                });

                await _messageService.CreateTopmaiPayMessage((Guid)payment.ToId, content, (int)Enums.MessageType.Pago);
            }

            _dBContext.SaveChanges();

            return _dBContext.Sells.First(w => w.Id == sell.Id);
        }

        public object Put(Sell newSell)
        {
            var idSell = newSell.Id;
            if (idSell == null || idSell.ToString().Length < 6) return new { error = "Ingrese un id de venta válido " };

            Sell? sell = _dBContext.Sells.Where(w => w.Id == idSell && newSell.Id != null).FirstOrDefault();
            if (sell == null) return new { error = "El id no coincide con ninguna venta " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newSell.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newSell) != null && propertyInfo.GetValue(newSell).ToString() != "")
                    propertyInfo.SetValue(sell, propertyInfo.GetValue(newSell));

            }


            _dBContext.Entry(sell).State = EntityState.Modified;

            _dBContext.SaveChanges();

            return sell;
        }

        public object Delete(Guid id)
        {

            Sell sell = _dBContext.Sells.FirstOrDefault(u => u.Id == id);
            if (sell == null)
                return new { error = "El id ingresado no es válido" };

            sell.Deleted = true;


            _dBContext.Entry(sell).State = EntityState.Modified;
            _dBContext.SaveChanges();

            return sell;
        }

        public object ChangeSellStatus(Guid id, int statusId)
        {
            Sell sell = _dBContext.Sells.FirstOrDefault(u => u.Id == id);
            if (sell == null)
                return new { error = "La venta no es válida" };

            Status status = _dBContext.Statuses.FirstOrDefault(s => s.Id == statusId);
            if (status == null)
                return new { error = "El estado de la venta no es válido" };

            if (status.Ambit != "Sell")
                return new { error = "El estado de la venta no es válido" };

            Devolution devolution = _dBContext.Devolutions
                                            .Include("Status")
                                            .FirstOrDefault(d => d.SellId == sell.Id);
            if (devolution != null && devolution.Status.Name != "Rechazada")
            {
                if (status.Name == "Recibida" || status.Name == "Enviada" || status.Name == "Entregada")
                    return new { error = "La venta no puede ser marcada como recibida o enviada porque tiene una solicitud de devolución abierta" };

            }

            StatusChange statusChange = _dBContext.StatusChanges
                                                  .FirstOrDefault(s => s.SellId == id && s.EndDate == null);
            if (statusChange != null)
            {
                statusChange.EndDate = DateTime.Now;
            }

            //Create new status change
            StatusChange newStatusChange = new StatusChange();
            newStatusChange.Id = Guid.NewGuid();
            newStatusChange.Deleted = false;
            newStatusChange.StatusId = statusId;
            newStatusChange.SellId = id;
            newStatusChange.StartDate = DateTime.Now;
            newStatusChange.EndDate = null;
            _dBContext.StatusChanges.Add(newStatusChange);

            if (status.Id == (int)Enums.State.Recibida_Sell)
            {
                Payment payment = _dBContext.Payments.FirstOrDefault(p => p.SellId == id);
                if (payment != null)
                {
                    if ((payment.ReceiptDate - DateTime.Now) > TimeSpan.FromDays(2))
                    {
                        payment.ReceiptDate = DateTime.Now + TimeSpan.FromDays(2);
                        _dBContext.Entry(payment).State = EntityState.Modified;

                    }

                }
            }
            sell.StatusId = statusId;
            _dBContext.Entry(sell).State = EntityState.Modified;

            _dBContext.SaveChanges();

            return sell;
        }

        public object GetSellsByProfileId(Guid id)
        {
            Profile profile = _dBContext.Profiles.FirstOrDefault(p => p.Id == id);
            if (profile == null)
                return new { error = "El perfil ingresado no es válido" };

            List<Sell> sells = _dBContext.Sells
            .Include("Publication")
            .Include("Publication.Currency")
            .Include("Status")
            .Where(s => s.SellerId == id)
            .OrderByDescending(s => s.DateTime).ToList()
            .ToList();


            return sells;
        }

        public object GetBuysByProfileId(Guid id)
        {
            Profile profile = _dBContext.Profiles.FirstOrDefault(p => p.Id == id);
            if (profile == null)
                return new { error = "El perfil ingresado no es válido" };

            List<Sell> sells = _dBContext.Sells
            .Include("Publication")
            .Include("Publication.Currency")
            .Include("Status")

            .Where(s => s.BuyerId == id)
            .OrderByDescending(s => s.DateTime).ToList()

            .ToList();


            return sells;
        }



        public object GetAmountOfSellsByProfileId(Guid id)
        {
            Profile profile = _dBContext.Profiles.Where(p => p.Id == id).FirstOrDefault();
            if (profile == null)
                return new { error = "El perfil ingresado no es válido" };

            return _dBContext.Sells.Where(s => s.SellerId == id).Count();
        }

        public object GetAmountOfBuysByProfileId(Guid id)
        {
            Profile profile = _dBContext.Profiles.Where(p => p.Id == id).FirstOrDefault();
            if (profile == null) return new { error = "El perfil ingresado no es válido" };
            return _dBContext.Sells.Where(s => s.BuyerId == id).Count();
        }

        public object GetAmountOfShippmentsByProfileId(Guid id)
        {
            Profile profile = _dBContext.Profiles.Where(p => p.Id == id).FirstOrDefault();
            if (profile == null) return new { error = "El perfil ingresado no es válido" };

            return _dBContext.Sells.Where(s => s.SellerId == id && s.ShippingCode.Length > 3).Count();
        }


        #endregion
    }
}
