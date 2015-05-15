using System;
using System.Web.Mvc;
using Kore.Plugins.Ecommerce.Simple.Models;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Mvc;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [RouteArea(Constants.RouteArea)]
    [RoutePrefix("paypal")]
    public class PayPalController : KoreController
    {
        private readonly Lazy<IProductService> productService;
        private readonly PayPalSettings settings;

        public PayPalController(Lazy<IProductService> productService, PayPalSettings settings)
        {
            this.productService = productService;
            this.settings = settings;
        }

        [Route("payment-completed")]
        public ActionResult PaymentCompleted(FormCollection formCollection)
        {
            // Temporarily pass formCollection as model, to see what is available
            return View(formCollection);
        }

        [Route("payment-cancelled")]
        public ActionResult PaymentCancelled(FormCollection formCollection)
        {
            // Temporarily pass formCollection as model, to see what is available
            return View(formCollection);
        }

        [Route("payment-notification")]
        public ActionResult PaymentNotification(FormCollection formCollection)
        {
            // Temporarily pass formCollection as model, to see what is available
            return View(formCollection);
        }

        [Route("buy-now")]
        public ActionResult BuyNow(int productId)
        {
            var product = productService.Value.FindOne(productId);
            if (product == null)
            {
                return HttpNotFound();
            }

            var model = new PayPalModel
            {
                UseSandboxMode = settings.UseSandboxMode,
                ActionUrl = settings.UseSandboxMode
                    ? settings.SandboxUrl
                    : settings.ProductionUrl,
                Merchant = settings.Merchant,
                ProductId = productId,
                ProductName = product.Name,
                CurrencyCode = settings.CurrencyCode,
                Amount = product.Price + product.Tax + product.ShippingCost,
                PaymentCompletedUrl = Url.AbsoluteAction("PaymentCompleted", "PayPal", null),
                PaymentCancelledUrl = Url.AbsoluteAction("PaymentCancelled", "PayPal", null),
                PaymentNotificationUrl = Url.AbsoluteAction("PaymentNotification", "PayPal", null)
            };
            return View(model);
        }
    }
}