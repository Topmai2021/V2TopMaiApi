using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Locations;
using Infraestructure.Entity.Entities.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.RegularExpressions;
using TopMai.Domain.Services.Locations.Interfaces;

namespace TopMai.Domain.Services.Locations
{
    public class CountryService : ICountryService
    {
        private DataContext DBContext;
        #region Builder
        public CountryService(DataContext dBContext)
        {
            this.DBContext = dBContext;

        }
        #endregion

        #region Methods
        public List<Country> GetAll()
        {
            List<Country> countries = DBContext.Countries.OrderByDescending(x => x.Id).ToList();

            return countries;


        }

        public Country Get(Guid id)
        {
            return DBContext.Countries.FirstOrDefault(g => g.Id == id);

        }

        public object Post(Country country)
        {
            country.Id = Guid.NewGuid();
            country.Deleted = false;

            if (country.Name == null || country.Name.Length < 3) return new { error = "El nombre del pais debe ser de al menos 3 caracteres " };
            if (!Regex.Match(country.Name, "^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$").Success) return new { error = "El nombre no puede tener caracteres especiales" };

            DBContext.Countries.Add(country);
            DBContext.SaveChanges();
            return DBContext.Countries.Where(c => c.Id == country.Id).First();


        }

        public object Put(Country newCountry)
        {
            var idCountry = newCountry.Id;
            if (idCountry == null || idCountry.ToString().Length < 6) return new { error = "Ingrese un id de pais válido " };

            Country? country = DBContext.Countries.Where(c => c.Id == idCountry && newCountry.Id != null).FirstOrDefault();
            if (country == null) return new { error = "El id no coincide con ningun pais " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newCountry.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newCountry) != null && propertyInfo.GetValue(newCountry).ToString() != "")
                    propertyInfo.SetValue(country, propertyInfo.GetValue(newCountry));

            }

            DBContext.Entry(country).State = EntityState.Modified;
            DBContext.SaveChanges();
            return country;


        }
        public object Delete(Guid id)
        {

            Country country = DBContext.Countries.FirstOrDefault(g => g.Id == id);
            if (country == null) return new { error = "El id ingresado no es válido" };

            country.Deleted = true;
            DBContext.Entry(country).State = EntityState.Modified;
            DBContext.SaveChanges();
            return country;
        }

        public bool NameIsRepeated(string name)
        {
            var repeatName = (Gender?)DBContext.Genders.Where(g => g.Name == name).FirstOrDefault();
            if (repeatName != null) return true;
            return false;

        }
        #endregion
    }
}
