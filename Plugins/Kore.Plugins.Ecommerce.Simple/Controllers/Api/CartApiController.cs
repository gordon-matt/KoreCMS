//using System.Linq;
//using System;
//using System.Collections.Generic;
//using System.Web.Http;
//using System.Web.Http.OData;
//using System.Web.Http.OData.Query;
//using Kore.Plugins.Ecommerce.Simple.Models;
//using Kore.Plugins.Ecommerce.Simple.Services;
//using System.Net;
//using System.Web;

//namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
//{
//    public class CartApiController : ODataController
//    {
//        private readonly Lazy<IProductService> productService;

//        public CartApiController(Lazy<IProductService> productService)
//        {
//            this.productService = productService;
//        }

//        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
//        public IQueryable<ShoppingCartItem> Get()
//        {
//            return GetCart().AsQueryable();
//        }

//        public IHttpActionResult Post(int productId)
//        {
//            var product = productService.Value.FindOne(productId);

//            if (product == null)
//            {
//                return NotFound();
//            }

//            var cart = GetCart();

//            var item = new ShoppingCartItem
//            {
//                ProductId = product.Id,
//                ProductName = product.Name,
//                Price = product.Price,
//                Quantity = 1
//            };
//            cart.Add(item);

//            UpdateCart(cart);

//            return Created(item);
//        }

//        public IHttpActionResult Put([FromODataUri] int productId, int quantity)
//        {
//            var product = productService.Value.FindOne(productId);

//            if (product == null)
//            {
//                return NotFound();
//            }

//            var cart = GetCart();

//            var item = cart.FirstOrDefault(x => x.ProductId == productId);

//            if (item != null)
//            {
//                item.Quantity = quantity;
//            }
//            else
//            {
//                item = new ShoppingCartItem
//                {
//                    ProductId = product.Id,
//                    ProductName = product.Name,
//                    Price = product.Price,
//                    Quantity = quantity
//                };
//                cart.Add(item);
//            }
//            UpdateCart(cart);

//            return Updated(item);
//        }

//        public IHttpActionResult Delete([FromODataUri] int productId)
//        {
//            var cart = GetCart();
//            var item = cart.FirstOrDefault(x => x.ProductId == productId);

//            if (item == null)
//            {
//                return NotFound();
//            }

//            cart.Remove(item);
//            UpdateCart(cart);

//            return StatusCode(HttpStatusCode.NoContent);
//        }

//        private ICollection<ShoppingCartItem> GetCart()
//        {
//            ICollection<ShoppingCartItem> cart;

//            if (HttpContext.Current.Session.Keys.OfType<string>().Contains("ShoppingCart"))
//            {
//                cart = (ICollection<ShoppingCartItem>)HttpContext.Current.Session["ShoppingCart"];
//            }
//            else
//            {
//                cart = new List<ShoppingCartItem>();
//            }

//            return cart;
//        }

//        private void UpdateCart(ICollection<ShoppingCartItem> cart)
//        {
//            HttpContext.Current.Session["ShoppingCart"] = cart;
//        }
//    }
//}