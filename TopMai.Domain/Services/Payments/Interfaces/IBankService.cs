using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IBankService
    {
        List<Bank> GetAll();
        Bank Get(Guid id);

        Task<object> Post(Bank bank);

        Task<object> Put(Bank newBank);

        Task<object> Delete(Guid id);
    }
}
