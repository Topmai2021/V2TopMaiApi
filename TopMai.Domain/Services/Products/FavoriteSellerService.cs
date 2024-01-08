using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Products.Interfaces;

namespace TopMai.Domain.Services.Products
{
    public class FavoriteSellerService : IFavoriteSellerService
    {
        private DataContext DBContext;

        #region Builder
        public FavoriteSellerService(DataContext dBContext)
        {
            DBContext = dBContext;
        }
        #endregion

        #region Methods
        public List<FavoriteSeller> GetAll() => DBContext.FavoriteSellers.OrderByDescending(x => x.Id).ToList();

        public FavoriteSeller Get(Guid id) => DBContext.FavoriteSellers.FirstOrDefault(p => p.Id == id);

        public object Post(FavoriteSeller favoriteSeller)
        {

            favoriteSeller.Id = Guid.NewGuid();
            favoriteSeller.Deleted = false;
            favoriteSeller.DateTime = DateTime.Now;


            if (favoriteSeller.ProfileId == favoriteSeller.SellerId)
                return new { error = "El id de perfil no puede coincidir con el de vendedor" };

            if (favoriteSeller.ProfileId == null || favoriteSeller.ProfileId.ToString().Length < 6)
                return new { error = "La url de la favoriteSellern debe ser de al menos 5 caracteres " };

            Profile profile = DBContext.Profiles.FirstOrDefault(p => p.Id == favoriteSeller.ProfileId);
            if (profile == null)
                return new { error = "El id no coincide con ningun perfil " };

            Profile seller = DBContext.Profiles.FirstOrDefault(p => p.Id == favoriteSeller.SellerId);
            if (seller == null)
                return new { error = "El id no coincide con ningun vendedor " };
            FavoriteSeller repeatedFavoriteSeller = DBContext.FavoriteSellers.FirstOrDefault(f => f.ProfileId == favoriteSeller.ProfileId && f.SellerId == favoriteSeller.SellerId);
            if (repeatedFavoriteSeller != null)
            {
                if (repeatedFavoriteSeller.Deleted == true)
                {
                    repeatedFavoriteSeller.Deleted = false;
                    repeatedFavoriteSeller.DateTime = DateTime.Now;
                    DBContext.FavoriteSellers.Update(repeatedFavoriteSeller);
                    DBContext.SaveChanges();
                    return repeatedFavoriteSeller;
                }
                else
                    return new { error = "El perfil ya tiene agregado el vendedor " };
            }


            DBContext.FavoriteSellers.Add(favoriteSeller);
            DBContext.SaveChanges();

            return DBContext.FavoriteSellers.First(p => p.Id == favoriteSeller.Id);
        }

        public object GetFavoriteSellersByProfileId(Guid id)
        {
            Profile profile = DBContext.Profiles.Where(p => p.Id == id).FirstOrDefault();
            if (profile == null)
                return new { error = "El id no coincide con ningun perfil " };
            List<FavoriteSeller> favoriteSellers = DBContext.FavoriteSellers.Where(f => f.ProfileId == id && f.Deleted != true).ToList();
            foreach (FavoriteSeller favoriteSeller in favoriteSellers)
            {
                favoriteSeller.Seller = DBContext.Profiles.Where(p => p.Id == favoriteSeller.SellerId).FirstOrDefault();
            }

            return favoriteSellers;
        }

        public object Put(FavoriteSeller newFavoriteSeller)
        {
            var idFavoriteSeller = newFavoriteSeller.Id;
            if (idFavoriteSeller == null || idFavoriteSeller.ToString().Length < 6) return new { error = "Ingrese un id de favoriteSellern válido " };

            FavoriteSeller? favoriteSeller = DBContext.FavoriteSellers.Where(c => c.Id == idFavoriteSeller && newFavoriteSeller.Id != null).FirstOrDefault();
            if (favoriteSeller == null) return new { error = "El id no coincide con ninguna favoriteSellern " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newFavoriteSeller.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newFavoriteSeller) != null && propertyInfo.GetValue(newFavoriteSeller).ToString() != "")
                {
                    propertyInfo.SetValue(favoriteSeller, propertyInfo.GetValue(newFavoriteSeller));

                }

            }

            DBContext.Entry(favoriteSeller).State = EntityState.Modified;
            DBContext.SaveChanges();

            return favoriteSeller;
        }

        public object Delete(Guid id)
        {
            FavoriteSeller favoriteSeller = DBContext.FavoriteSellers.FirstOrDefault(p => p.Id == id);
            if (favoriteSeller == null)
                return new { error = "El id ingresado no es válido " };

            favoriteSeller.Deleted = true;
            DBContext.Entry(favoriteSeller).State = EntityState.Modified;
            DBContext.SaveChanges();

            return favoriteSeller;
        }
        #endregion
    }
}
