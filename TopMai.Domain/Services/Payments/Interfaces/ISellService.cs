using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Products;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface ISellService
    {
        SellsResult GetAll(int pageNumber, int pageSize);
        Sell Get(Guid id);
        Task<Sell> Post(Sell sell, int paymentMethodId);
        object Put(Sell newSell);
        object Delete(Guid id);
        object GetSellsByProfileId(Guid id);
        object GetBuysByProfileId(Guid id);
        object GetAmountOfSellsByProfileId(Guid id);
        object GetAmountOfBuysByProfileId(Guid id);
        object GetAmountOfShippmentsByProfileId(Guid id);
        object ChangeSellStatus(Guid id, int statusId);
        System.IO.Stream GetTracker(string id);
        Task<Stream> CreateTracker (string carrier, string? trackingCode);

    }
}
