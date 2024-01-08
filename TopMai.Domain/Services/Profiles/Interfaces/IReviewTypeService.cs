using Infraestructure.Entity.Entities.Profiles;

namespace TopMai.Domain.Services.Profiles.Interfaces
{
    public interface IReviewTypeService
    {
        List<ReviewType> GetAll();
        object Get(Guid? id);
        object Post(ReviewType reviewType);
        object Put(ReviewType newReviewType);
        object Delete(Guid id);

    }
}
