using Infraestructure.Entity.DTOs;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Request;
using TopMai.Domain.DTO.Products;

namespace TopMai.Domain.Services.Products.Interfaces
{
    public interface IPublicationService
    {
        Task<PublicationResult> GetAll(int Page = 1, int Limit = 10);
        // Task<List<Publication>> GetAll(int Page = 1, int Limit = 10);
        Task<Publication> Get(Guid id);
        List<ConsultPublication_Dto> GetHomePublications();
        object GetActivePublications(int? page);
        object Post(PublicationDTO publication);
        object Put(PublicationUpdateRequest newPublication);
        object RemoveMultiplePublications(List<Guid> ids);
        object RenewMultiplePublications(List<Guid> ids);
        object Delete(Guid id);
        object GetPublicationsBySubcategory(Guid id);
        Category? GetCategory(Publication publication);
        object GetPublicationsByCategory(MessageRequest request);
        object GetPublicationsByProfile(Guid id);
        List<Image> GetImagesByPublication(Guid id);
        object AddShippmentTypeToPublication(int idShippmentType, Guid idPublication, float? price);
        object GetShippmentTypeByPublication(Guid id);
        Task<Guid> AddNewVisit(Publication publication);
        Task<Guid> AddNewVisitEndPoint(Guid id);
        List<Publication> DefaultSearch(string query);
        object SearchPublication(string query, int filter);
        object AddImageToPublication(Guid idImage, Guid idPublication, int? number);

        object RemoveImagePublications(Guid idPublication);
    }
}
