using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IPinService
    {
        List<Pin> GetAll();
        Pin Get(Guid id);

        Task<object> Post(Pin bank);

        Task<object> Put(Pin newPin);
        bool ValidatePin(Guid userId,string pin);
        Task<object> Delete(Guid id);
    }
}
