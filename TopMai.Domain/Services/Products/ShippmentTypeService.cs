using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TopMai.Domain.Services.Products.Interfaces;

namespace TopMai.Domain.Services.Products
{
    public class ShippmentTypeService : IShippmentTypeService
    {
        private DataContext DBContext;

        #region Builder
        public ShippmentTypeService(DataContext dBContext)
        {
            DBContext = dBContext;
        }
        #endregion

        #region Methods
        public List<ShippmentType> GetAll() => DBContext.ShippmentTypes.OrderBy(x => x.Id).ToList();

        public ShippmentType Get(int id) => DBContext.ShippmentTypes.FirstOrDefault(u => u.Id == id);

        public object Post(ShippmentType shippmentType)
        {
            shippmentType.Deleted = false;
            if (shippmentType.Name == null || shippmentType.Name.Length < 3)
                return new { error = "El nombre debe ser de al menos 3 caracteres " };

            if (!Regex.Match(shippmentType.Name, "^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$").Success)
                return new { error = "El nombre no puede tener caracteres especiales" };

            DBContext.ShippmentTypes.Add(shippmentType);
            DBContext.SaveChanges();

            return (DBContext.ShippmentTypes.Where(w => w.Id == shippmentType.Id).First());
        }

        public object Delete(int id)
        {
            ShippmentType shippmentType = DBContext.ShippmentTypes.FirstOrDefault(c => c.Id == id);
            if (shippmentType == null)
                return new { error = "El id ingresado no es válido" };

            shippmentType.Deleted = true;
            DBContext.Entry(shippmentType).State = EntityState.Modified;
            DBContext.SaveChanges();

            return shippmentType;
        }

        public object Put(ShippmentType newShippmentType)
        {
            var idShippmentType = newShippmentType.Id;
            if (idShippmentType == null || idShippmentType.ToString().Length < 6)
                return new { error = "Ingrese un id de tipo de envio válido " };

            ShippmentType? shippmentType = DBContext.ShippmentTypes.Where(c => c.Id == idShippmentType && newShippmentType.Id != null).FirstOrDefault();
            if (shippmentType == null)
                return new { error = "El id no coincide con ningun tipo de envio " };


            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newShippmentType.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newShippmentType) != null && propertyInfo.GetValue(newShippmentType).ToString() != "")
                    propertyInfo.SetValue(shippmentType, propertyInfo.GetValue(newShippmentType));

            }

            DBContext.Entry(shippmentType).State = EntityState.Modified;
            DBContext.SaveChanges();

            return shippmentType;
        }
        #endregion
    }
}
