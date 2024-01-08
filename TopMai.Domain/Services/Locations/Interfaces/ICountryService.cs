using Infraestructure.Entity.Entities.Locations;

namespace TopMai.Domain.Services.Locations.Interfaces
{
    public interface ICountryService
    {
        List<Country> GetAll();
        Country Get(Guid id);
        object Post(Country country);
        object Put(Country newCountry);
        object Delete(Guid id);
        bool NameIsRepeated(string name);
    }
}
