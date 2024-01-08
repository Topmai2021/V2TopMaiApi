using Infraestructure.Entity.Entities.Products;

namespace TopMai.Domain.Services.Products.Interfaces
{
    public interface IFavoriteSellerService
    {
        List<FavoriteSeller> GetAll();
        FavoriteSeller Get(Guid id);
        object Post(FavoriteSeller favoriteSeller);
        object GetFavoriteSellersByProfileId(Guid id);
        object Put(FavoriteSeller newFavoriteSeller);
        object Delete(Guid id);
    }
}
