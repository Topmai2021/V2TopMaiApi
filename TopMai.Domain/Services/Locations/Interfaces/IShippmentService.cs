using Infraestructure.Entity.Entities.Locations;

namespace TopMai.Domain.Services.Locations.Interfaces
{
    public interface IShippmentService
    {
        List<Shippment> GetAll();
        Shippment Get(Guid id);

        object Post(Shippment shippment);

        object Put(Shippment newShippment);

        object Delete(Guid? id);
    }
}
