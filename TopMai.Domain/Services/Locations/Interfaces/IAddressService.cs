using Infraestructure.Entity.Entities.Locations;

namespace TopMai.Domain.Services.Locations.Interfaces
{
    public interface IAddressService
    {
        List<Address> GetAll();
        Address Get(Guid id);
        object Post(Address address);
        object Put(Address newAddress);
        object Delete(Guid? id);
        object GetAddressesByProfileId(Guid? id);
    }
}
