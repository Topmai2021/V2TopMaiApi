using Infraestructure.Entity.Entities.Products;

namespace TopMai.Domain.Services.Products.Interfaces
{
    public interface IShippmentTypeService
    {
        List<ShippmentType> GetAll();
        ShippmentType Get(int id);
        object Post(ShippmentType shippmentType);
        object Delete(int id);
        object Put(ShippmentType newShippmentType);
    }
}
