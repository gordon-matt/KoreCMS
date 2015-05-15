using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Plugins.Ecommerce.Simple.Models;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Mvc;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [RouteArea(Constants.RouteArea)]
    [RoutePrefix("paypal")]
    public class PayPalController : KoreController
    {
        private readonly Lazy<IOrderService> orderService;
        private readonly Lazy<IProductService> productService;
        private readonly PayPalSettings settings;

        public PayPalController(
            Lazy<IOrderService> orderService,
            Lazy<IProductService> productService,
            PayPalSettings settings)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.settings = settings;
        }

        [Route("payment-completed/{orderId}")]
        public ActionResult PaymentCompleted(int orderId, FormCollection formCollection)
        {
            // Temporarily pass formCollection as model, to see what is available
            return View(formCollection);
        }

        [Route("payment-cancelled/{orderId}")]
        public ActionResult PaymentCancelled(int orderId, FormCollection formCollection)
        {
            // Temporarily pass formCollection as model, to see what is available
            return View(formCollection);
        }

        [Route("payment-notification/{orderId}")]
        public ActionResult PaymentNotification(int orderId, FormCollection formCollection)
        {
            // Temporarily pass formCollection as model, to see what is available
            return View(formCollection);
        }

        [Route("buy-now")]
        public ActionResult BuyNow()
        {
            var cart = GetCart();

            var order = new Order
            {
                UserId = WorkContext.CurrentUser == null ? null : WorkContext.CurrentUser.Id,
                PaymentStatus = PaymentStatus.Pending,
                Status = OrderStatus.Pending,
                OrderDateUtc = DateTime.UtcNow,
            };

            foreach (var item in cart)
            {
                order.Lines.Add(new OrderLine
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                });
            }

            orderService.Value.Insert(order);

            var model = new PayPalModel
            {
                UseSandboxMode = settings.UseSandboxMode,
                ActionUrl = settings.UseSandboxMode
                    ? settings.SandboxUrl
                    : settings.ProductionUrl,
                Merchant = settings.Merchant,
                CurrencyCode = settings.CurrencyCode,
                Items = cart,
                //ProductId = productId,
                //ProductName = product.Name,
                //Amount = product.Price + product.Tax + product.ShippingCost,
                PaymentCompletedUrl = Url.AbsoluteAction("PaymentCompleted", "PayPal", new { orderId = order.Id }),
                PaymentCancelledUrl = Url.AbsoluteAction("PaymentCancelled", "PayPal", new { orderId = order.Id }),
                PaymentNotificationUrl = Url.AbsoluteAction("PaymentNotification", "PayPal", new { orderId = order.Id })
            };
            return View(model);
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
    }
}