using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kore.Plugins.Ecommerce.Simple.Models;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Mvc;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [RouteArea("")]
    [RoutePrefix("store/cart")]
    public class CartController : KoreController
    {
        private readonly Lazy<IProductService> productService;

        public CartController(Lazy<IProductService> productService)
        {
            this.productService = productService;
        }

        [Route("get-cart")]
        public JsonResult Get()
        {
            var cart = GetCart();
            return Json(new { Items = cart }, JsonRequestBehavior.AllowGet);
        }

        [Route("add-item-to-cart")]
        public JsonResult Post(int productId)
        {
            var product = productService.Value.FindOne(productId);

            if (product == null)
            {
                return Json(new { Success = false, Message = "Could not find specified product" });
            }

            var cart = GetCart();

            var item = new ShoppingCartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1
            };
            cart.Add(item);

            UpdateCart(cart);

            return Json(new { Success = true, Message = "Added to Cart" });
        }

        [Route("update-cart-item/{productId}")]
        public JsonResult Put(int productId, int quantity)
        {
            var product = productService.Value.FindOne(productId);

            if (product == null)
            {
                return Json(new { Success = false, Message = "Could not find specified product" });
            }

            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                item.Quantity = quantity;
            }
            else
            {
                item = new ShoppingCartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                };
                cart.Add(item);
            }
            UpdateCart(cart);

            return Json(new { Success = true, Message = "Cart Updated" });
        }

        [Route("delete-cart-item/{productId}")]
        public JsonResult Delete(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
            {
                return Json(new { Success = false, Message = "Could not find specified product" });
            }

            cart.Remove(item);
            UpdateCart(cart);

            return Json(new { Success = true, Message = "Item removed from cart" });
        }

        private ICollection<ShoppingCartItem> GetCart()
        {
            ICollection<ShoppingCartItem> cart;

            if (Session.Keys.OfType<string>().Contains("ShoppingCart"))
            {
                cart = (ICollection<ShoppingCartItem>)Session["ShoppingCart"];
            }
            else
            {
                cart = new List<ShoppingCartItem>();
            }

            return cart;
        }

        private void UpdateCart(ICollection<ShoppingCartItem> cart)
        {
            Session["ShoppingCart"] = cart;
        }
    }
}