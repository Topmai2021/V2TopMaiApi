using Infraestructure.Entity.Entities.Products;

namespace TopMai.Domain.Services.Products.Interfaces
{
    public interface IWeightService
    {
        List<Weight> GetAll();
        Weight Get(Guid id);
        object Post(Weight weight);
        object Put(Weight newWeight);
        object Delete(Guid id);
    }
}
