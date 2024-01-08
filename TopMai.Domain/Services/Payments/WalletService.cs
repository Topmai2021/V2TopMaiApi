using Common.Utils.Enums;
using Common.Utils.Exceptions;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Profiles;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Openpay;
using Openpay.Entities;
using Openpay.Entities.Request;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Domain.Services.Emails.Interfaces;
using TopMai.Domain.Services.Payments.Interfaces;
using Card = Infraestructure.Entity.Entities.Payments.Card;
using User = Infraestructure.Entity.Entities.Users.User;

namespace TopMai.Domain.Services.Payments
{
    public class WalletService : IWalletService
    {
        #region Attributes
        private DataContext DBContext;
        private IMessageService _messageService;
        private IMovementService _movementService;
        private IEmailService _emailService;
        private IStorePayRequestService _storePayRequestService;

        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Builder
        public WalletService(DataContext dBContext,
            IMessageService messageService,
            IMovementService movementService,
            IEmailService emailService,
            IStorePayRequestService storePayRequestService,

            IUnitOfWork unitOfWork)

        {
            DBContext = dBContext;
            _messageService = messageService;
            _movementService = movementService;
            _emailService = emailService;
            _storePayRequestService = storePayRequestService;
            _unitOfWork = unitOfWork;
        }
        #endregion



  public IEnumerable<Wallet> GetAllWalletRecordsWithProfiles()
    {
        // Implement logic to fetch all wallet records with associated profiles
        var walletRecords = _unitOfWork.WalletRepository.GetAll();

        return walletRecords;
    }
        public float GetBalance(Guid idWallet, Guid idUser)
        {
            //El IdUser es el mismo IdProfile
            Wallet wallet = _unitOfWork.WalletRepository.FirstOrDefault(w => w.Id == idWallet
                                                                          && w.Profiles.Any(x => x.Id == idUser));

            if (wallet == null)
                throw new BusinessException("Solo el propietario de la Wallet puede realizar la operación.");


            return wallet.Money;
        }

        public Wallet Get(Guid id) => _unitOfWork.WalletRepository.FirstOrDefault(w => w.Id == id);

        public async Task<bool> Post(Wallet wallet)
        {
            wallet.Money = 0;
            _unitOfWork.WalletRepository.Insert(wallet);

            return await _unitOfWork.Save() > 0;
        }

        public async Task<bool> UpdateWallet(Wallet wallet)
        {
            _unitOfWork.WalletRepository.Update(wallet);

            return await _unitOfWork.Save() > 0;
        }
        public float getPendingAmountByUserId(Guid profileId)
        {

            float payments = (float)DBContext.Payments.Where(p => p.ToId == profileId
                                                && p.Deleted == false
                                                && p.StatusId == (int)Enums.State.Pendiente_Payment).Sum(p => p.TotalWithoutCommission);

            float movements = (float)DBContext.Movements.Where(m => m.ProfileId == profileId
                                                && m.Deleted == false
                                                && m.MovementTypeId == (int)Enums.MovementType.Input
                                                && m.StatusId == (int)Enums.State.Pendiente_Movement).Sum(m => m.Amount);
            return payments + movements;
        }
        public object Delete(Guid id)
        {
            Wallet wallet = DBContext.Wallets.FirstOrDefault(u => u.Id == id);
            if (wallet == null)
                return new { error = "El id ingresado no es válido" };

            //wallet.Deleted = true;
            DBContext.Entry(wallet).State = EntityState.Modified;
            DBContext.SaveChanges();

            return wallet;
        }


        public bool HasPin(Guid id)
        {
            User user = DBContext.Users.Include("Profile").FirstOrDefault(u => u.Id == id);
            if (user == null)
                return false;

            user.Profile.Wallet = DBContext.Wallets.FirstOrDefault(w => w.Id == user.Profile.WalletId);
            if (user.Profile.Wallet == null)
                return false;

            return (user.Profile.Wallet.Pin != null && user.Profile.Wallet.Pin != "");
        }

        public float CalculateCommission(float amount, string paymentMethod)
        {
            switch (paymentMethod)
            {
                case "credit": return (float)(0.4F + (amount * 0.0047F) + (amount * 0.029F) + 2.5F);
                case "debit": return (float)(0.4F + (amount * 0.0047F) + (amount * 0.029F) + 2.5F);

                case "oxxo": return (float)((amount * 0.00624F) + (amount * 0.039F));
                default: return 14.5F;
            }
        }



        public async Task<Payment> Transfer(Guid idFrom, Guid idTo, float amount)
        {
            Profile profileFrom = DBContext.Profiles.Where(u => u.Id == idFrom).First();
            if (profileFrom == null)
                throw new BusinessException("El id remitente no es válido");

            Profile profileTo = DBContext.Profiles.Where(u => u.Id == idTo).First();
            if (profileTo == null)
                throw new BusinessException("El id receptor no es valido");


            Wallet walletFrom = DBContext.Wallets.Where(w => w.Id == profileFrom.WalletId).First();
            Wallet walletTo = DBContext.Wallets.Where(w => w.Id == profileTo.WalletId).First();
            if (walletFrom.Money < amount)
                throw new BusinessException("La billetera del usuario remitente no tiene fondos suficientes");

            walletFrom.Money -= amount;
            walletTo.Money += amount;

            Payment payment = new Payment();
            payment.Id = Guid.NewGuid();
            payment.DateHour = DateTime.Now;
            payment.ReceiptDate = DateTime.Now;
            payment.Total = amount;
            payment.FromId = idFrom;
            payment.ToId = idTo;

            payment.StatusId = (int)Enums.State.Acreditado_Payment;
            payment.PaymentMethodId = (int)Enums.PaymentMethod.Instantaneo;

            payment.CurrencyId = (int)Enums.Currency.MXN;

            string content = JsonConvert.SerializeObject(new
            {
                from = profileFrom.ProfileUrl,
                to = profileTo.ProfileUrl,
                amount = amount,
                currency = "MXN",
                date = payment.DateHour,
                receiptDate = payment.ReceiptDate,
                status = "Acreditado",
                type = "Transferencia",
                id = payment.Id,
                movementType = "Payment"
            });
            await _messageService.CreateTopmaiPayMessage((Guid)payment.ToId, content, (int)Enums.MessageType.Pago);
            DBContext.Payments.Add(payment);
            DBContext.SaveChanges();

            return payment;
        }



        public async Task<int> AcreditPayments()
        {
            List<Payment> payments = DBContext.Payments.Include("Status")
                .Where(p => (((DateTime)p.ReceiptDate) <= DateTime.Now) && p.Status.Name == "Pendiente" && p.ReceiptDate != null).ToList();
            if (payments.Count == 0)
                return 1;

            foreach (Payment payment in payments)
            {
                payment.ReceiptDate = DateTime.Now;
                Profile profileTo = DBContext.Profiles.Where(u => u.Id == payment.ToId).First();
                Profile profileFrom = DBContext.Profiles.Where(u => u.Id == payment.FromId).First();
                Wallet walletTo = DBContext.Wallets.Where(w => w.Id == profileTo.WalletId).First();
                walletTo.Money += (float)payment.TotalWithoutCommission;

                payment.StatusId = (int)Enums.State.Acreditado_Payment;

                Sell sell = DBContext.Sells.Include("Status").FirstOrDefault(s => s.Id == payment.SellId);
                if (sell != null)
                {
                    if (sell.Status.Name != "Recibida")
                    {

                        sell.StatusId = (int)Enums.State.Recibida_Sell;

                        // create status change
                        sell.StatusChanges = DBContext.StatusChanges.Where(sc => sc.SellId == sell.Id && sc.EndDate == null).ToList();
                        if (sell.StatusChanges.Count > 0)
                        {
                            foreach (StatusChange statusCh in sell.StatusChanges)
                            {
                                statusCh.EndDate = DateTime.Now;
                            }
                        }
                        StatusChange statusChange = new StatusChange();
                        statusChange.Id = Guid.NewGuid();
                        statusChange.StartDate = DateTime.Now;
                        statusChange.StatusId = sell.StatusId;
                        statusChange.SellId = sell.Id;
                        DBContext.StatusChanges.Add(statusChange);

                    }

                    var res = DBContext.SaveChanges();
                    if (res != 0)
                    {
                        string content = JsonConvert.SerializeObject(new
                        {
                            from = profileFrom.ProfileUrl,
                            to = profileTo.ProfileUrl,
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

                }


            }

            DBContext.SaveChanges();
            return 1;
        }

        public object PayInStore(Guid profileId, decimal total)
        {
            Profile profile = DBContext.Profiles.Where(p => p.Id == profileId).FirstOrDefault();
            if (profile == null)
                return new { error = "El perfil no es válido" };

            Boolean production = false;
            OpenpayAPI openpayAPI = new OpenpayAPI("sk_ed50cb213356462faa3d139c2f27938f", "mfz0rmrjpxpihzlwtimp", production);

            ChargeRequest request = new ChargeRequest();




            request.Method = "store";
            request.Amount = total;
            request.Description = "Ingreso de dinero en tienda";


            Customer customer = new Customer();
            if (profile.OpenPayCustomerId != null)
            {
                customer = openpayAPI.CustomerService.Get((string)profile.OpenPayCustomerId);
            }
            else
            {
                //create customer
                customer = new Customer();
                customer.Name = profile.Name;
                customer.LastName = profile.LastName;
                customer.PhoneNumber = profile.Phone;
                customer.Email = "topmai.com.mx@gmail.com";
                customer.ExternalId = profile.Id.ToString();
                profile.OpenPayCustomerId = openpayAPI.CustomerService.Create(customer).Id;
                DBContext.SaveChanges();


            }


            request.Metadata = new Dictionary<string, string>();
            request.Metadata.Add("profileId", profileId.ToString());


            try
            {
                Charge charge = openpayAPI.ChargeService.Create(profile.OpenPayCustomerId.ToString(), request);

                StorePayRequest storePayRequest = new StorePayRequest();
                storePayRequest.Id = Guid.NewGuid();
                storePayRequest.ProfileId = profileId;
                storePayRequest.Amount = (float?)total;
                storePayRequest.BarCodeUrl = charge.PaymentMethod.BarcodeURL;
                storePayRequest.Reference = charge.PaymentMethod.Reference;
                storePayRequest.DateTime = DateTime.Now;


                DBContext.StorePayRequests.Add(storePayRequest);
                DBContext.SaveChanges();

                return charge;


            }
            catch (Exception e)
            {
                return new { error = e.Message };
            }

        }
        public async Task<object> PayWithCard(Guid cardId, Guid profileId, decimal total, string deviceSessionId)
        {
            Boolean production = false;
            OpenpayAPI openpayAPI = new OpenpayAPI("sk_ed50cb213356462faa3d139c2f27938f", "mfz0rmrjpxpihzlwtimp", production);

            Card card = DBContext.Cards.FirstOrDefault(c => c.Id == cardId && c.Deleted != true);
            if (card == null)
                return new { error = "La tarjeta no es válida" };
            Profile profile = DBContext.Profiles.FirstOrDefault(p => p.Id == profileId
                                                                && p.Deleted != true);
            if (profile == null)
            {
                return new { error = "El perfil no es válido" };

            }

            if (profile.OpenPayCustomerId == null)
            {
                //create customer
                Customer customer = new Customer();
                customer.Name = profile.Name;
                customer.LastName = profile.LastName;
                customer.PhoneNumber = profile.Phone;
                customer.Email = "topmai.com.mx@gmail.com";
                customer.ExternalId = profile.Id.ToString();
                profile.OpenPayCustomerId = openpayAPI.CustomerService.Create(customer).Id;
                DBContext.SaveChanges();
            }

            var openPayCardId = "";
            if (card.OpenPayCardId == null)
            {
                // create card
                Openpay.Entities.Card openPayCard = new Openpay.Entities.Card();
                openPayCard.CardNumber = (string)card.Number.ToString();
                openPayCard.BankCode = (string)card.SecurityCode.ToString();
                openPayCard.ExpirationMonth = card.ExpirationMonth.ToString();
                openPayCard.ExpirationYear = card.ExpirationYear.ToString();
                try
                {
                    openPayCard.Cvv2 = EncryptProvider.Base64Decrypt(card.SecurityCode.ToString()).ToString();


                }
                catch (Exception e)
                {
                    openPayCard.Cvv2 = (string)card.SecurityCode.ToString();

                }
                openPayCard.HolderName = card.FullName;
                try
                {
                    card.OpenPayCardId = openpayAPI.CardService.Create(profile.OpenPayCustomerId, openPayCard).Id;
                    openPayCardId = card.OpenPayCardId;
                    DBContext.SaveChanges();

                }
                catch (Exception e)
                {
                    return new { error = e.Message };
                }
            }
            else
            {
                openPayCardId = card.OpenPayCardId;
            }


            //create charge request 
            ChargeRequest request = new ChargeRequest();
            request.Method = "card";

            request.Amount = total;
            request.Description = "Ingreso de dinero topmai";
            request.Currency = "MXN";

            request.Metadata = new Dictionary<string, string>();
            request.Metadata.Add("profileId", profileId.ToString());

            request.SourceId = openPayCardId;
            request.DeviceSessionId = deviceSessionId;

            //create charge
            try
            {
                Charge charge = openpayAPI.ChargeService.Create(profile.OpenPayCustomerId, request);

                Movement repeatedMovement = DBContext.Movements.FirstOrDefault(m => m.ConektaOrderId == charge.OrderId
                    && m.MovementTypeId == (int)Enums.MovementType.Input);
                if (repeatedMovement != null)
                {
                    return new { error = "La transacción ya fue procesada" };
                }

                if (charge.Status == "completed")
                {
                    //add money to wallet

                    Movement movement = new Movement();
                    movement.MovementTypeId = (int)Enums.MovementType.Input;
                    movement.Amount = (float)(request.Amount - (request.Amount * 0.03m) - 2.5m - (0.4m + (request.Amount * 0.0047m)));
                    movement.ProfileId = profileId;
                    movement.ConektaOrderId = charge.OrderId;

                    movement.StatusId = (int)Enums.State.Pendiente_Movement;
                    _movementService.Post(movement);
                    Movement updatedMovement = new Movement();
                    updatedMovement.Id = movement.Id;

                    updatedMovement.StatusId = (int)Enums.State.Aprobado_Movement;
                    await _movementService.Put(updatedMovement);
                    movement.StatusId = (int)Enums.State.Aprobado_Movement;
                    await _movementService.Put(updatedMovement);

                }

                if (charge.Status == "in_progress")
                {
                    // create movement request
                    Movement movement = new Movement();
                    movement.MovementTypeId = (int)Enums.MovementType.Input;
                    movement.Amount = (float)(request.Amount - (request.Amount * 0.03m) - 2.5m - (0.4m + (request.Amount * 0.0047m)));
                    movement.ProfileId = profileId;
                    movement.ConektaOrderId = charge.OrderId;

                    movement.StatusId = (int)Enums.State.Pendiente_Movement;
                    _movementService.Post(movement);

                }

                return charge;
            }
            catch (Exception e)
            {
                return new { error = e.Message };
            }





        }

        public async Task<object> OpenPayWebHook(string request)
        {
            Boolean production = false;
            OpenpayAPI openpayAPI = new OpenpayAPI("sk_ed50cb213356462faa3d139c2f27938f", "mfz0rmrjpxpihzlwtimp", production);



            var obj = JObject.Parse(request.ToString());
            //_emailService.SendEmail("topmai.com.mx@gmail.com","Prueba codigo openpay",request.ToString());

            var type = obj.SelectToken("type");
            if (type.ToString() == "verification")
            {
                var verificationCode = obj.SelectToken("verification_code");
                //_emailService.SendEmail("topmai.com.mx@gmail.com","Open pay Webhook verification code",verificationCode.ToString());

            }
            else
            {
                var transaction = obj.SelectToken("transaction");
                switch (type.ToString())
                {
                    case "charge.succeeded":
                        {
                            var orderId = transaction.SelectToken("id").ToString();
                            try
                            {
                                var customer_id = transaction.SelectToken("customer_id").ToString();
                                Profile profile = DBContext.Profiles.FirstOrDefault(p => p.OpenPayCustomerId == customer_id);

                                if (profile != null)
                                {
                                    Movement repeated = DBContext.Movements.Include("Status").FirstOrDefault(m => m.ConektaOrderId == orderId
                                                && (Guid)m.ProfileId == Guid.Parse(profile.Id.ToString()));
                                    if (repeated != null)
                                    {
                                        if (repeated.Status.Name == "Pendiente")
                                        {
                                            Movement updatedMovement = new Movement();
                                            updatedMovement.Id = repeated.Id;

                                            updatedMovement.StatusId = (int)Enums.State.Aprobado_Movement;
                                            await _movementService.Put(updatedMovement);
                                        }

                                    }
                                    else
                                    {
                                        Movement movement = new Movement();
                                        movement.MovementTypeId = (int)Enums.MovementType.Input;
                                        movement.ProfileId = Guid.Parse(profile.Id.ToString());
                                        movement.ConektaOrderId = orderId;
                                        var total = decimal.Parse(transaction.SelectToken("amount").ToString());
                                        movement.Amount = (float)(total - (total * 0.03m) - 2.5m - (0.4m + (total * 0.0047m)));


                                        _movementService.Post(movement);

                                        Movement updatedMovement = new Movement();
                                        updatedMovement.Id = movement.Id;

                                        updatedMovement.StatusId = (int)Enums.State.Aprobado_Movement;
                                        await _movementService.Put(updatedMovement);

                                    }


                                }
                            }
                            catch (Exception e)
                            {

                            }

                        }

                        ; break;
                    case ("chargeback.accepted"):
                        {
                            CreateChargeback(transaction);
                        }; break;
                    case ("charge.refunded"):
                        {
                            CreateChargeback(transaction);
                        }; break;

                }

            }

            return request.ToString();

        }

        public async void CreateChargeback(JToken transaction)
        {
            var orderId = transaction.SelectToken("id").ToString();
            try
            {
                var customer_id = transaction.SelectToken("customer_id").ToString();
                Profile profile = DBContext.Profiles.FirstOrDefault(p => p.OpenPayCustomerId == customer_id);

                if (profile != null)
                {
                    Movement repeated = DBContext.Movements.Include("Status").FirstOrDefault(m => m.ConektaOrderId == orderId
                                && (Guid)m.ProfileId == Guid.Parse(profile.Id.ToString())
                                && m.MovementTypeId == (int)Enums.MovementType.Output);
                    if (repeated != null)
                    {
                        if (repeated.Status.Name == "Pendiente")
                        {
                            Movement updatedMovement = new Movement();
                            updatedMovement.Id = repeated.Id;
                            updatedMovement.StatusId = (int)Enums.State.Aprobado_Movement;
                            await _movementService.Put(updatedMovement);
                        }
                    }
                    else
                    {
                        Movement movement = new Movement();
                        movement.MovementTypeId = (int)Enums.MovementType.Output;
                        movement.ProfileId = Guid.Parse(profile.Id.ToString());
                        movement.ConektaOrderId = orderId;
                        var total = decimal.Parse(transaction.SelectToken("amount").ToString());
                        movement.Amount = (float)(total - (total * 0.03m) - 2.5m - (0.4m + (total * 0.0047m)));


                        _movementService.Post(movement);

                        Movement updatedMovement = new Movement();
                        updatedMovement.Id = movement.Id;
                        updatedMovement.StatusId = (int)Enums.State.Aprobado_Movement;
                        await _movementService.Put(updatedMovement);

                    }
                }
            }
            catch (Exception e)
            {

            }

        }
        public object CreateWalletPin(Guid idUser, int pin)
        {
            User user = DBContext.Users.Include("Profile").Where(u => u.Id == idUser).First();
            if (user == null)
                return new { error = "El usuario no es valido" };

            Wallet wallet = DBContext.Wallets.Where(w => w.Id == user.Profile.WalletId).First();
            if (wallet == null)
                return new { error = "El usuario no tiene una billetera" };

            if (wallet.Pin != null)
                return new { error = "El usuario ya tiene un pin" };

            if (pin < 100000 || pin > 999999)
                return new { error = "El pin debe ser de 6 digitos" };

            wallet.Pin = EncryptProvider.Sha1(pin.ToString());
            DBContext.SaveChanges();

            return wallet;
        }

        public object ChangePin(Guid idUser, int oldPin, int newPin)
        {
            User user = DBContext.Users.Include("Profile").Where(u => u.Id == idUser).First();
            if (user == null)
                return new { error = "El usuario no es valido" };

            Wallet wallet = DBContext.Wallets.Where(w => w.Id == user.Profile.WalletId).First();
            if (wallet == null)
                return new { error = "El usuario no tiene una billetera" };

            if (wallet.Pin != EncryptProvider.Sha1(oldPin.ToString()))
                return new { error = "El pin actual no es correcto" };

            if (newPin < 100000 || newPin > 999999)
                return new { error = "El pin debe ser de 6 digitos" };

            wallet.Pin = EncryptProvider.Sha1(newPin.ToString());
            DBContext.SaveChanges();

            return wallet;
        }
        public object RecoverPin(Guid idUser, int pin)
        {
            User user = DBContext.Users.Include("Profile").Where(u => u.Id == idUser).First();
            if (user == null)
                return new { error = "El usuario no es valido" };

            Wallet wallet = DBContext.Wallets.Where(w => w.Id == user.Profile.WalletId).First();
            if (wallet == null)
                return new { error = "El usuario no tiene una billetera" };

            if (wallet.Pin == null)
                return new { error = "El usuario no tiene un pin" };

            if (pin < 100000 || pin > 999999)
                return new { error = "El pin debe ser de 6 digitos" };

            var encriptedPin = EncryptProvider.Sha1(pin.ToString());
            if (wallet.Pin == encriptedPin)
                return new { error = "El pin es igual al anterior" };

            wallet.Pin = encriptedPin;
            DBContext.SaveChanges();

            return wallet;
        }
        public object ValidatePin(Guid idUser, int pin)
        {
            User user = DBContext.Users.Include("Profile").Where(u => u.Id == idUser).First();
            if (user == null)
                return new { error = "El usuario no es valido" };

            Wallet wallet = DBContext.Wallets.Where(w => w.Id == user.Profile.WalletId).First();
            if (wallet == null)
                return new { error = "El usuario no tiene una billetera" };

            if (wallet.Pin == null)
                return new { error = "El usuario no tiene un pin" };

            if (wallet.Pin != EncryptProvider.Sha1(pin.ToString()))
                return new { error = "El pin no es valido" };

            return wallet;
        }



        public object getTotalBalance()
        {
            float input = (float)DBContext.Movements
                                            .Where(m =>
                                            m.MovementTypeId == (int)Enums.MovementType.Input
                                            && m.StatusId == (int)Enums.State.Aprobado_Movement)
                                            .Sum(m => m.Amount);

            float output = (float)DBContext.Movements
                                            .Where(m => m.MovementTypeId == (int)Enums.MovementType.Output
                                            && m.StatusId == (int)Enums.State.Aprobado_Movement)
                                            .Sum(m => m.Amount);

            float commissions = (float)DBContext.Payments.Where(p => p.StatusId == (int)Enums.State.Acreditado_Payment)
                                            .Sum(p => p.Total - p.TotalWithoutCommission);

            float pendingCommissions = (float)DBContext.Payments.Where(p => p.StatusId == (int)Enums.State.Pendiente_Payment)
                                                        .Sum(p => p.Total - p.TotalWithoutCommission);
            return new { input = input, output = output, balance = input - output, commissions = commissions, pendingCommissions = pendingCommissions };

        }


        //        public object GeneratePaymentUrl(string email, int amount, Guid userId)
        //        {
        //            if (amount < 2000) return new { error = "El monto debe ser mayor a 20.00 (2000)" };

        //            conekta.Api.apiKey = "key_twRutnxNxtE9Ss6JgpwtEA";
        //            conekta.Api.version = "2.0.0";
        //            conekta.Api.locale = "es";

        //            User user = DBContext.Users.Where(u => u.Id == userId).First();
        //            if (user == null) return new { error = "El usuario no es valido" };
        //            Guid walletId = Guid.Parse(user.WalletId.ToString());
        //            if (walletId == null || walletId.ToString().Length < 6) return new { error = "El usuario no tiene una billetera" };

        //            var customerString = @"{
        //                ""name"":""Payment link"",
        //                ""email"":""#email#""            }

        //";
        //            customerString = customerString.Replace("#email#", email);

        //            var customer = new Customer().create(customerString);

        //            string customerId = customer.id.ToString();


        //            string stringOrder = @"{
        //                      ""currency"":""MXN"",
        //                      ""customer_info"": {
        //                      ""customer_id"": ""#customer_id#""

        //                      },
        //                      ""line_items"": [{
        //                      ""name"": ""Carga de billetera "",
        //                      ""unit_price"": #amount# ,
        //                      ""quantity"": 1,
        //                      ""metadata"":{
        //                                ""walletId"":""#walletId#""
        //                                    }
        //                      }],


        //                    ""checkout"":{
        //                        ""allowed_payment_methods"": [""cash"",""bank_transfer""],
        //                        ""type"":""HostedPayment"",
        //                        ""success_url"" : ""https://sysgestion.somee.com/wallet/success"",
        //                        ""failure_url"": ""https://sysgestion.somee.com/wallet/failure"",
        //                        ""monthly_installments_enabled"": false,
        //                        ""redirection_time"": 20

        //                        }
        //                         }";

        //            stringOrder = stringOrder.Replace("#customer_id#", customerId);
        //            stringOrder = stringOrder.Replace("#amount#", amount.ToString());
        //            stringOrder = stringOrder.Replace("#walletId#", walletId.ToString());

        //            conekta.Order order = new conekta.Order().create(stringOrder);
        //            var checkoutRequestId = order.checkout.id;

        //            string url = "https://sysgestion.somee.com/api/Wallet/checkout?checkoutRequestId=" + checkoutRequestId.ToString();

        //            return order.checkout.url;
        //        }

    }
}
