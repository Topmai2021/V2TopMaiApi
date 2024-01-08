using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IStorePayRequestService
    {
        List<StorePayRequest> GetAll();
        StorePayRequest Get(Guid id);

        Task<object> Post(StorePayRequest storePayRequest);

        Task<object> Put(StorePayRequest newStorePayRequest);

        List<StorePayRequest> GetStorePayRequestsByProfile(Guid profileId);

        Task<object> Delete(Guid id);
    }
}
