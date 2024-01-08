using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Products.Interfaces;

namespace TopMai.Domain.Services.Products
{
    public class FavoritePublicationService : IFavoritePublicationService
    {
        private DataContext DBContext;

        #region Builder
        public FavoritePublicationService(DataContext dBContext)
        {
            DBContext = dBContext;
        }
        #endregion

        #region Methods
        public List<FavoritePublication> GetAll() => DBContext.FavoritePublications.OrderByDescending(x => x.Id).ToList();

        public FavoritePublication Get(Guid id) => DBContext.FavoritePublications.FirstOrDefault(u => u.Id == id);

        public object Post(FavoritePublication favoritePublication)
        {
            if (favoritePublication.ProfileId == null || favoritePublication.ProfileId.ToString().Length < 3)
                return new { error = "El intervalo del peso debe ser al menos 3 caracteres" };

            Profile profile = DBContext.Profiles.FirstOrDefault(p => p.Id == favoritePublication.ProfileId);
            if (profile == null)
                return new { error = "El id no coincide con ningun perfil" };
            Publication publication = DBContext.Publications.FirstOrDefault(p => p.Id == favoritePublication.PublicationId);
            if (publication == null)
                return new { error = "El id no coincide con ninguna publicación" };
            FavoritePublication repeatedFavoritePublication = DBContext.FavoritePublications.FirstOrDefault(p => p.ProfileId == favoritePublication.ProfileId && p.PublicationId == favoritePublication.PublicationId);
            if (repeatedFavoritePublication != null)
                return new { error = "El perfil ya ha agregado esta publicación como favorita" };
            favoritePublication.Id = Guid.NewGuid();
            favoritePublication.Deleted = false;
            favoritePublication.DateTime = DateTime.Now;

            DBContext.FavoritePublications.Add(favoritePublication);
            DBContext.SaveChanges();

            return DBContext.FavoritePublications.Where(w => w.Id == favoritePublication.Id).First();
        }

        public object Put(FavoritePublication newFavoritePublication)
        {
            var idFavoritePublication = newFavoritePublication.Id;
            if (idFavoritePublication == null || idFavoritePublication.ToString().Length < 6)
                return new { error = "Ingrese un id de peso válido " };

            FavoritePublication? favoritePublication = DBContext.FavoritePublications.FirstOrDefault(w => w.Id == idFavoritePublication && newFavoritePublication.Id != null);
            if (favoritePublication == null)
                return new { error = "El id no coincide con ningun peso " };

            //loop through each attribute entered and modify it
            foreach (PropertyInfo propertyInfo in newFavoritePublication.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newFavoritePublication) != null && propertyInfo.GetValue(newFavoritePublication).ToString() != "")
                    propertyInfo.SetValue(favoritePublication, propertyInfo.GetValue(newFavoritePublication));
            }

            DBContext.Entry(favoritePublication).State = EntityState.Modified;
            DBContext.SaveChanges();

            return favoritePublication;
        }

        public object Delete(Guid id)
        {
            FavoritePublication favoritePublication = DBContext.FavoritePublications.FirstOrDefault(u => u.Id == id);

            if (favoritePublication == null)
                return new { error = "El id ingresado no es válido" };

            favoritePublication.Deleted = true;
            DBContext.Entry(favoritePublication).State = EntityState.Modified;
            DBContext.SaveChanges();

            return favoritePublication;
        }

        public object GetFavoritePublicationsByProfile(Guid id)
        {
            Profile profile = DBContext.Profiles.FirstOrDefault(p => p.Id == id);
            if (profile == null)
                return new { error = "El id no coincide con ningun perfil" };
            List<FavoritePublication> favoritePublications = DBContext.FavoritePublications.Where(w => w.ProfileId == id && w.Deleted != true).ToList();
            foreach (FavoritePublication favoritePublication in favoritePublications)
            {
                favoritePublication.Publication = DBContext.Publications.FirstOrDefault(p => p.Id == favoritePublication.PublicationId);
            }

            return favoritePublications;
        }

        #endregion
    }
}
