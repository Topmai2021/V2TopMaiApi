using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Products.Interfaces;

namespace TopMai.Domain.Services.Products
{
    public class CartService : ICartService
    {
        private DataContext DBContext;

        #region Builder
        public CartService(DataContext dBContext)
        {
            DBContext = dBContext;
        }
        #endregion

        #region Methods
        public List<Cart> GetAll() => DBContext.Carts.OrderByDescending(x => x.Id).ToList();
        public Cart Get(Guid id) => DBContext.Carts.FirstOrDefault(u => u.Id == id);

        public object Post(Cart cart)
        {
            if (cart.ProfileId == null || cart.ProfileId.ToString().Length < 6)
                return new { error = "Ingrese un id de perfil válido" };

            Profile profile = DBContext.Profiles.FirstOrDefault(p => p.Id == cart.ProfileId);
            if (profile == null)
                return new { error = "No se encuentra ningun perfil con la id ingresada" };

            cart.Id = Guid.NewGuid();
            cart.Deleted = false;

            DBContext.Carts.Add(cart);
            DBContext.SaveChanges();

            return DBContext.Carts.Where(w => w.Id == cart.Id).First();
        }

        public object Put(Cart newCart)
        {
            var idCart = newCart.Id;
            if (idCart == null || idCart.ToString().Length < 6)
                return new { error = "Ingrese un id de carrito válido " };

            Cart? cart = DBContext.Carts.Where(w => w.Id == idCart && newCart.Id != null).FirstOrDefault();
            if (cart == null)
                return new { error = "El id no coincide con ningun carrito " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newCart.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newCart) != null && propertyInfo.GetValue(newCart).ToString() != "")
                {
                    propertyInfo.SetValue(cart, propertyInfo.GetValue(newCart));

                }

            }

            DBContext.Entry(cart).State = EntityState.Modified;
            DBContext.SaveChanges();

            return cart;
        }

        public object Delete(Guid id)
        {
            Cart cart = DBContext.Carts.FirstOrDefault(u => u.Id == id);
            if (cart == null)
                return new { error = "El id ingresado no es válido" };

            cart.Deleted = true;
            DBContext.Entry(cart).State = EntityState.Modified;
            DBContext.SaveChanges();

            return cart;
        }
        #endregion
    }
}
