using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Locations;
using Infraestructure.Entity.Entities.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Locations.Interfaces;

namespace TopMai.Domain.Services.Locations
{
    public class AddressService : IAddressService
    {
        private DataContext DBContext;
        #region Builder
        public AddressService(DataContext dBContext)
        {
            this.DBContext = dBContext;

        }
        #endregion


        #region Methods
        public List<Address> GetAll()
        {
            List<Address> addresses = DBContext.Addresses.OrderByDescending(x => x.Id).ToList();

            return addresses;
        }

        public Address Get(Guid id)
        {
            return DBContext.Addresses.FirstOrDefault(a => a.Id == id);

        }

        public object Post(Address address)
        {

            address.Id = Guid.NewGuid();
            address.Deleted = false;
            Profile profile = DBContext.Profiles.FirstOrDefault(p => p.Id == address.ProfileId);
            if (profile == null)
                return new { message = "El perfil no existe" };
            
            
            if (address.Street == null || address.Street.Length < 4) return new { error = "La calle debe ser de al menos 3 caracteres " };

            if(address.Active == true)
            {
                List<Address> addresses = DBContext.Addresses.Where(x => x.ProfileId == address.ProfileId).ToList();
                foreach (var item in addresses)
                {
                    item.Active = false;
                }
            address.Active = true;

            }

            DBContext.Addresses.Add(address);
            DBContext.SaveChanges();
            return DBContext.Addresses.Where(a => a.Id == address.Id).First();


        }

        public object Put(Address newAddress)
        {
            var idAddress = newAddress.Id;
            if (idAddress == null || idAddress.ToString().Length < 6) return new { error = "Ingrese un id de direccion válido " };

            Address? address = DBContext.Addresses.Where(a => a.Id == idAddress && newAddress.Id != null).FirstOrDefault();
            if (address == null) return new { error = "El id no coincide con ninguna direccion " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newAddress.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newAddress) != null && propertyInfo.GetValue(newAddress).ToString() != "")
                {
                    propertyInfo.SetValue(address, propertyInfo.GetValue(newAddress));

                }

            }

            if(address.Active == true)
            {
                List<Address> addresses = DBContext.Addresses.Where(x => x.ProfileId == address.ProfileId).ToList();
                foreach (var item in addresses)
                {
                    item.Active = false;
                }
            address.Active = true;

            }


            DBContext.Entry(address).State = EntityState.Modified;
            DBContext.SaveChanges();
            return address;


        }

        public object Delete(Guid? id)
        {

            Address address = DBContext.Addresses.FirstOrDefault(u => u.Id == id);
            if (address == null) return new { error = "El id ingresado no coincide con ninguna direccion" };
            address.Deleted = true;
            DBContext.Entry(address).State = EntityState.Modified;
            DBContext.SaveChanges();
            return address;
        }

        public object GetAddressesByProfileId(Guid? id)
        {
            if (id == null || id.ToString().Length < 6) return new { error = "Ingrese un id de perfil válido " };
            Profile profile = DBContext.Profiles.FirstOrDefault(p => p.Id == id);
            if (profile == null) return new { error = "El id ingresado no coincide con ningun perfil" };

            List<Address> addresses = DBContext.Addresses.Where(a => a.ProfileId == id).ToList();
            return addresses;
        } 
        #endregion
    }
}
