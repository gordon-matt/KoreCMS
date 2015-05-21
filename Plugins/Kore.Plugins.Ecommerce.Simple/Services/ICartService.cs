using System.Linq;
using System.Web;
using Kore.Plugins.Ecommerce.Simple.Models;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface ICartService
    {
        CartModel GetCart(HttpContextBase context);

        void SetCart(HttpContextBase context, CartModel cart);
    }

    public class CartService : ICartService
    {
        #region ICartService Members

        public CartModel GetCart(HttpContextBase context)
        {
            CartModel cart;

            if (context.Session.Keys.OfType<string>().Contains("ShoppingCart"))
            {
                cart = (CartModel)context.Session["ShoppingCart"];
            }
            else
            {
                cart = new CartModel();
            }

            return cart;
        }

        public void SetCart(HttpContextBase context, CartModel cart)
        {
            context.Session["ShoppingCart"] = cart;
        }

        #endregion ICartService Members
    }
}