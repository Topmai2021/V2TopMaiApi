using Infraestructure.Entity.Entities.Payments;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IWalletService
    {
        float GetBalance(Guid idWallet, Guid idUser);

        IEnumerable<Wallet> GetAllWalletRecordsWithProfiles();
        Wallet Get(Guid id);
        Task<bool> Post(Wallet wallet);
        Task<bool> UpdateWallet(Wallet wallet);
        object Delete(Guid id);
        bool HasPin(Guid id);
        float CalculateCommission(float amount, string paymentMethod);
        // bool IsOrderPayed(string id);
        Task<Payment> Transfer(Guid idFrom, Guid idTo, float amount);
        //object PaySell(Guid idSell);
        Task<int> AcreditPayments();

        object CreateWalletPin(Guid idUser, int pin);
        object ChangePin(Guid idUser, int oldPin, int newPin);

        object RecoverPin(Guid idUser, int pin);
        object ValidatePin(Guid idUser, int pin);

        object getTotalBalance();
        float getPendingAmountByUserId(Guid userId);
        Task<object> OpenPayWebHook(string request);

        Task<object> PayWithCard(Guid cardId, Guid profileId, decimal total, string deviceSessionId);
        object PayInStore(Guid profileId, decimal total);
        // object GeneratePaymentUrl(string email, int amount, Guid userId);
    }
}
