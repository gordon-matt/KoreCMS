using System;
using System.Linq;
using System.Web.Mvc;
using Kore.Collections;
using Kore.Plugins.Ecommerce.Simple.Models;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Mvc;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [RouteArea("")]
    [RoutePrefix("store/cart")]
    public class CartController : KoreController
    {
        private readonly ICartService cartService;
        private readonly Lazy<IProductService> productService;
        private readonly Lazy<StoreSettings> settings;

        public CartController(
            ICartService cartService,
            Lazy<IProductService> productService,
            Lazy<StoreSettings> settings)
        {
            this.cartService = cartService;
            this.productService = productService;
            this.settings = settings;
        }

        [Route("")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store), Url.Action("Index", "Store", new { area = string.Empty }));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.ShoppingCart));

            ViewBag.CurrencyCode = settings.Value.Currency;
            return View();
        }

        [Route("get-cart")]
        public JsonResult Get()
        {
            var cart = cartService.GetCart(this.HttpContext);
            return Json(new { Items = cart.Items }, JsonRequestBehavior.AllowGet);
        }

        [Route("add-item-to-cart")]
        public JsonResult Post(int productId)
        {
            var cart = cartService.GetCart(this.HttpContext);

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                item.Quantity += 1;
                cartService.SetCart(this.HttpContext, cart);
                return Json(new { Success = true, Message = T(LocalizableStrings.QuantityUpdated).Text });
            }

            var product = productService.Value.FindOne(productId);

            if (product == null)
            {
                return Json(new { Success = false, Message = T(LocalizableStrings.CouldNotFindProduct).Text });
            }

            item = new ShoppingCartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1,
                ShippingCost = product.ShippingCost,
                Tax = product.Tax,
                ImageUrl = product.MainImageUrl,
                Description = product.ShortDescription.Left(255)
            };
            cart.Items.Add(item);

            cartService.SetCart(this.HttpContext, cart);

            return Json(new { Success = true, Message = T(LocalizableStrings.AddToCartSuccess).Text });
        }

        [Route("update-cart-item/{productId}")]
        public JsonResult Put(int productId, short quantity)
        {
            var product = productService.Value.FindOne(productId);

            if (product == null)
            {
                return Json(new { Success = false, Message = T(LocalizableStrings.CouldNotFindProduct).Text });
            }

            var cart = cartService.GetCart(this.HttpContext);

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);

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
                    Quantity = quantity,
                    ShippingCost = product.ShippingCost,
                    Tax = product.Tax,
                    ImageUrl = product.MainImageUrl,
                    Description = product.ShortDescription.Left(255)
                };
                cart.Items.Add(item);
            }
            cartService.SetCart(this.HttpContext, cart);

            return Json(new { Success = true, Message = T(LocalizableStrings.CartUpdated).Text });
        }

        [Route("delete-cart-item/{productId}")]
        public JsonResult Delete(int productId)
        {
            var cart = cartService.GetCart(this.HttpContext);
            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
            {
                return Json(new { Success = false, Message = T(LocalizableStrings.CouldNotFindProduct).Text });
            }

            cart.Items.Remove(item);
            cartService.SetCart(this.HttpContext, cart);

            return Json(new { Success = true, Message = T(LocalizableStrings.ItemRemovedFromCart).Text });
        }

        [Route("update-cart")]
        public JsonResult UpdateCart(CartModel cart)
        {
            if (cart.Items.IsNullOrEmpty())
            {
                cartService.SetCart(this.HttpContext, new CartModel());
            }
            else
            {
                cartService.SetCart(this.HttpContext, cart);
            }

            return Json(new { Success = true, Message = T(LocalizableStrings.CartUpdated).Text });
        }
    }
}