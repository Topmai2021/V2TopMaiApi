using Infraestructure.Entity.Entities.Products;

namespace TopMai.Domain.Services.Products.Interfaces
{
    public interface IFavoritePublicationService
    {
        List<FavoritePublication> GetAll();
        FavoritePublication Get(Guid id);
        object Post(FavoritePublication favoritePublication);
        object Put(FavoritePublication newFavoritePublication);
        object Delete(Guid id);
        object GetFavoritePublicationsByProfile(Guid id);
    }
}
