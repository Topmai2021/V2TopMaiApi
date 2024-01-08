using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.DTO.Transactions.RechargueWallet;

namespace TopMai.Domain.Services.Transactions.Interfaces
{
    public interface IRechargueWalletServices
    {
        List<ConsultAllRechargueWallet_Dto> GetAllRechargueWallet();
        List<ConsultAllRechargueWallet_Dto> GetAllRechargueWalletByStatus(int Status);
        Task<List<ConsultRechargueWallet_Dto>> GetAllRechargueByWallet(Guid idWallet, Guid idUser);
        Task<ResultReference_Dto> GetPaymentReference(ReferencePayment_Dto reference, Guid idUser);
        Task<bool> CancelRechargueWalletByUser(Guid idRechargue, Guid idUser);
        Task<bool> ConfirmPaymentReference(ConfirmPaymentReference_Dto confirm, Guid idUser);
        Task<bool> PaymentApproved(Guid idRechargue, Guid idUser);
    }
}
