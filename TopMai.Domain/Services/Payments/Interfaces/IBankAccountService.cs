using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Request;
using TopMai.Domain.DTO.Bank;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IBankAccountService
    {
        List<BankAccount> GetAll();
        BankAccount Get(Guid id);

        Task<object> Post(BankAccount bankAccount);

        Task<bool> Delete(Guid userId, Guid idBankAccount);
        List<BankAccount> GetByUser(Guid userId);

        Task<ConsultBankAccountDto> GetActualBankAccount();

        Task<bool> DefaultBankAccount(Guid userId, Guid idBankAccount);
    }
}
