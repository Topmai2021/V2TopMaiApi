using Infraestructure.Entity.Entities.Products;

namespace TopMai.Domain.Services.Products.Interfaces
{
    public interface IPublicationLikeService
    {
        List<PublicationLike> GetAll();
        PublicationLike Get(Guid id);
        Task<object> Post(PublicationLike publicationComment);
        Task<object> Put(PublicationLike newPublicationComment);
        Task<object> Delete(Guid id);

    }
}
