using Infraestructure.Entity.Entities.Products;

namespace TopMai.Domain.Services.Products.Interfaces
{
    public interface IPublicationAttributeService
    {
        List<PublicationAttribute> GetAll();

        PublicationAttribute Get(Guid id);

        Task<object> Post(PublicationAttribute publicationAttribute);

        Task<object> Put(PublicationAttribute publicationAttribute);

        Task<object> Delete(Guid id);
    }
}
