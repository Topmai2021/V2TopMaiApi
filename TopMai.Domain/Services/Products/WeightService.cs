using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Products.Interfaces;

namespace TopMai.Domain.Services.Products
{
    public class WeightService : IWeightService
    {
        private DataContext DBContext;

        #region Builder
        public WeightService(DataContext dBContext)
        {
            DBContext = dBContext;
        }
        #endregion

        #region Methods
        public List<Weight> GetAll() => DBContext.Weights.OrderByDescending(x => x.Id).ToList();

        public Weight Get(Guid id) => DBContext.Weights.FirstOrDefault(u => u.Id == id);

        public object Post(Weight weight)
        {
            if (weight.Range == null || weight.Range.Length < 3)
                return new { error = "El intervalo del peso debe ser al menos 3 caracteres" };

            weight.Id = Guid.NewGuid();
            weight.Deleted = false;

            DBContext.Weights.Add(weight);
            DBContext.SaveChanges();

            return DBContext.Weights.Where(w => w.Id == weight.Id).First();
        }

        public object Put(Weight newWeight)
        {
            var idWeight = newWeight.Id;
            if (idWeight == null || idWeight.ToString().Length < 6)
                return new { error = "Ingrese un id de peso válido " };

            Weight? weight = DBContext.Weights.Where(w => w.Id == idWeight && newWeight.Id != null).FirstOrDefault();
            if (weight == null)
                return new { error = "El id no coincide con ningun peso " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newWeight.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newWeight) != null && propertyInfo.GetValue(newWeight).ToString() != "")
                    propertyInfo.SetValue(weight, propertyInfo.GetValue(newWeight));

            }

            DBContext.Entry(weight).State = EntityState.Modified;
            DBContext.SaveChanges();

            return weight;
        }

        public object Delete(Guid id)
        {

            Weight weight = DBContext.Weights.FirstOrDefault(u => u.Id == id);
            if (weight == null)
                return new { error = "El id ingresado no es válido" };

            weight.Deleted = true;
            DBContext.Entry(weight).State = EntityState.Modified;
            DBContext.SaveChanges();

            return weight;
        }
        #endregion
    }
}
