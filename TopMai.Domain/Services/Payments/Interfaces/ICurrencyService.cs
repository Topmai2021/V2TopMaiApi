using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface ICurrencyService
    {
        List<Currency> GetAll();
        Currency Get(int id);

        Task<Currency> Post(Currency currency);

        Task<Currency> Put(Currency newCurrency);

        Task<bool> Delete(int id);
    }
}
