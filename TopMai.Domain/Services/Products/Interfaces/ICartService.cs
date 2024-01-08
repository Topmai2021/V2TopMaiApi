using Infraestructure.Entity.Entities.Products;

namespace TopMai.Domain.Services.Products.Interfaces
{
    public interface ICartService
    {
        List<Cart> GetAll();
        Cart Get(Guid id);
        object Post(Cart cart);
        object Put(Cart newCart);
        object Delete(Guid id);
    }
}
