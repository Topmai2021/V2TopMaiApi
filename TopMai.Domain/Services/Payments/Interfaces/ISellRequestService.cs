using Infraestructure.Entity.DTOs;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Request;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface ISellRequestService
    {
        List<SellRequest> GetAll();
        SellRequest Get(Guid id);
        Task<SellRequest> Post(SellRequestDTO sellRequest);
        Task<SellRequest> Put(SellRequest newSellRequest);
        object Delete(Guid id);
        object GetBuyOffersByUserId(Guid id);
        object GetSellOffersByUserId(Guid id);
        object AcceptSellOffer(Guid sellRequestId, Guid sellerId);
        object DeclineSellOffer(Guid sellRequestId, Guid sellerId);
        Task<Sell> ConfirmSellOffer(SellOfferRequest sellOfferRequest);

        object GetComissions(Guid publicationId, float total, bool? withShippment);


    }
}
