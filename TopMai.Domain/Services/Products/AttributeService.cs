using Common.Utils.Exceptions;
using Common.Utils.Helpers;
using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.RegularExpressions;
using TopMai.Domain.Services.Products.Interfaces;

namespace TopMai.Domain.Services.Products
{
    public class AttributeService : IAttributeService
    {
        private DataContext DBContext;

        #region Builder
        public AttributeService(DataContext dBContext)
        {
            DBContext = dBContext;
        }
        #endregion

        #region Methods

        public List<Infraestructure.Entity.Entities.Products.Attribute> GetAll() => DBContext.Attributes.OrderByDescending(x => x.Id).ToList();

        public Infraestructure.Entity.Entities.Products.Attribute Get(Guid id) => DBContext.Attributes.FirstOrDefault(u => u.Id == id);

        public object Post(Infraestructure.Entity.Entities.Products.Attribute attribute)
        {
            if (attribute.CategoryId == null || attribute.CategoryId.ToString().Length < 6)
                throw new BusinessException("El id de categoria no es válido");

            Helper.ValidateName(attribute.Name);

            Category category = DBContext.Categories.Where(c => c.Id == attribute.CategoryId).FirstOrDefault();
            if (category == null) return new { error = "No se encuentra ninguna categoria con la id ingresada" };

            attribute.Id = Guid.NewGuid();
            attribute.Deleted = false;

            DBContext.Attributes.Add(attribute);
            DBContext.SaveChanges();

            return DBContext.Attributes.Where(a => a.Id == attribute.Id).First();
        }

        public async Task<object> GetAttributesByCategoryId(Guid id)
        {
            List<Infraestructure.Entity.Entities.Products.Attribute> attributes = await DBContext.Attributes.Where(a => a.CategoryId == id && a.Deleted == false).ToListAsync();
            return attributes;
        }

        public object Put(Infraestructure.Entity.Entities.Products.Attribute newAttribute)
        {
            var idAttribute = newAttribute.Id;
            if (idAttribute == null || idAttribute.ToString().Length < 6) return new { error = "Ingrese un id de atributo válido " };

            Infraestructure.Entity.Entities.Products.Attribute? attribute = DBContext.Attributes.FirstOrDefault(a => a.Id == idAttribute && newAttribute.Id != null);
            if (attribute == null) return new { error = "El id no coincide con ningun atributo " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newAttribute.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newAttribute) != null && propertyInfo.GetValue(newAttribute).ToString() != "")
                    propertyInfo.SetValue(attribute, propertyInfo.GetValue(newAttribute));

            }

            DBContext.Entry(attribute).State = EntityState.Modified;
            DBContext.SaveChanges();

            return attribute;
        }

        public object Delete(Guid id)
        {

            Infraestructure.Entity.Entities.Products.Attribute attribute = DBContext.Attributes.FirstOrDefault(a => a.Id == id);
            if (attribute == null) return new { error = "El id ingresado no es válido" };
            attribute.Deleted = true;
            DBContext.Entry(attribute).State = EntityState.Modified;
            DBContext.SaveChanges();

            return attribute;
        }
        #endregion
    }
}
