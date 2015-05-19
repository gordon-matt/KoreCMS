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
        public ActionResult PaymentCompleted(int orderId)
        {
            var order = orderService.Value.FindOne(orderId);

            if (order == null)
            {
                Logger.ErrorFormat("Could not find order number: {0}", orderId);
                return HttpNotFound();
            }

            order.Status = OrderStatus.Processing;
            order.PaymentStatus = PaymentStatus.Paid;

            var requestParams = Request.Params;
            Logger.InfoFormat("PaymentCompleted() Params: ",
                string.Join(", ", requestParams.Keys.OfType<string>().Select(x => x + ":" + requestParams[x])));

            // Temporarily pass formCollection as model, to see what is available
            return View(requestParams);
            //return View(order);
        }

        [Route("payment-cancelled/{orderId}")]
        public ActionResult PaymentCancelled(int orderId)
        {
            var order = orderService.Value.FindOne(orderId);

            if (order == null)
            {
                Logger.ErrorFormat("Could not find order number: {0}", orderId);
                return HttpNotFound();
            }

            order.Status = OrderStatus.Cancelled;

            var requestParams = Request.Params;
            Logger.InfoFormat("PaymentCancelled() Params: ",
                string.Join(", ", requestParams.Keys.OfType<string>().Select(x => x + ":" + requestParams[x])));

            // Temporarily pass formCollection as model, to see what is available
            return View(requestParams);
            //return View(order);
        }

        [Route("payment-notification/{orderId}")]
        public ActionResult PaymentNotification(int orderId)
        {
            var order = orderService.Value.FindOne(orderId);

            if (order == null)
            {
                Logger.ErrorFormat("Could not find order number: {0}", orderId);
                return HttpNotFound();
            }

            var requestParams = Request.Params;
            Logger.InfoFormat("PaymentNotification() Params: ",
                string.Join(", ", requestParams.Keys.OfType<string>().Select(x => x + ":" + requestParams[x])));

            // Temporarily pass formCollection as model, to see what is available
            return View(requestParams);
            //return View(order);
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

            //TEMP
            var address = new Address
            {
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                AddressLine3 = "Line 3",
                City = "Tel Aviv",
                Country = "Israel",
                PostalCode = "jhskaj",
                Email = "rn@esales.co.il",
                FamilyName = "Nissan",
                GivenNames = "Ran",
                PhoneNumber = "0987654321",
            };

            order.BillingAddress = address;
            order.ShippingAddress = address;
            //END TEMP

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