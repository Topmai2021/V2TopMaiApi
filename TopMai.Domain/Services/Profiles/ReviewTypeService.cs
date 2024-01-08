using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Profiles.Interfaces;

namespace TopMai.Domain.Services.Profiles
{
    public class ReviewTypeService : IReviewTypeService
    {
        private DataContext DBContext;

        #region Builder
        public ReviewTypeService(DataContext dBContext)
        {
            DBContext = dBContext;
        }
        #endregion

        #region Methods
        public List<ReviewType> GetAll()=> DBContext.ReviewTypes.OrderByDescending(x => x.Id).ToList();

        public object Get(Guid? id)
        {
            var reviewType = DBContext.ReviewTypes.FirstOrDefault(u => u.Id == id);
            if (reviewType == null) return new { value = "Perfil no encontrado" };

            return reviewType;
        }

        public object Post(ReviewType reviewType)
        {
            if (reviewType.Name == null || reviewType.Name.Length < 3) return new { error = "El nombre debe ser al menos 3 caracteres" };

            reviewType.Id = Guid.NewGuid();
            reviewType.Deleted = false;

            DBContext.ReviewTypes.Add(reviewType);
            DBContext.SaveChanges();
            ReviewType? res = DBContext.ReviewTypes.Where(p => p.Id == reviewType.Id).FirstOrDefault();

            return (res);
        }

        public object Put(ReviewType newReviewType)
        {
            var idReviewType = newReviewType.Id;
            if (idReviewType == null || idReviewType.ToString().Length < 6) return new { error = "Ingrese un id de reseña válido " };

            ReviewType? reviewType = DBContext.ReviewTypes.Where(c => c.Id == idReviewType && newReviewType.Id != null).FirstOrDefault();
            if (reviewType == null) return new { error = "El id no coincide con ninguna reseña" };
            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newReviewType.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newReviewType) != null && propertyInfo.GetValue(newReviewType).ToString() != "")
                    propertyInfo.SetValue(reviewType, propertyInfo.GetValue(newReviewType));

            }

            DBContext.Entry(reviewType).State = EntityState.Modified;
            DBContext.SaveChanges();

            return reviewType;
        }

        public object Delete(Guid id)
        {
            ReviewType reviewType = DBContext.ReviewTypes.FirstOrDefault(u => u.Id == id);
            if (reviewType == null)
                return new { error = "El id ingresado no es válido" };

            reviewType.Deleted = true;
            DBContext.Entry(reviewType).State = EntityState.Modified;
            DBContext.SaveChanges();

            return reviewType;
        } 
        #endregion
    }
}
