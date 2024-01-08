using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface ICardService
    {
        List<Card> GetAll();
        Card Get(Guid id);

        Task<object> Post(Card card);

        Task<object> Put(Card newCard);

        List<Card> GetCardsByProfile(Guid profileId);

        Task<object> Delete(Guid id);
    }
}
