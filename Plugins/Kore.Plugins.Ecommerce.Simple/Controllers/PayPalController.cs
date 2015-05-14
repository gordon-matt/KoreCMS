using System;
using System.Web.Mvc;
using Kore.Plugins.Ecommerce.Simple.Models;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Mvc;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [RouteArea(Constants.RouteArea)]
    [Route("paypal")]
    public class PayPalController : KoreController
    {
        private readonly Lazy<IProductService> productService;
        private readonly PayPalSettings settings;

        public PayPalController(Lazy<IProductService> productService, PayPalSettings settings)
        {
            this.productService = productService;
            this.settings = settings;
        }

        [Route("redirect")]
        public ActionResult RedirectFromPayPal(FormCollection formCollection)
        {
            return View();
        }

        [Route("cancel")]
        public ActionResult CancelFromPayPal(FormCollection formCollection)
        {
            return View();
        }

        [Route("notify")]
        public ActionResult NotifyFromPayPal(FormCollection formCollection)
        {
            return View();
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
                ProductName = product.Name,
                Amount = product.Price + product.Tax + product.ShippingCost,
                Command = "_xclick",
                Business = settings.Business,
                ActionUrl = settings.UseSandboxMode
                    ? settings.SandboxUrl
                    : settings.ProductionUrl,
                RedirectUrl = Url.AbsoluteAction("RedirectFromPayPal", "PayPal", null),
                CancelUrl = Url.AbsoluteAction("CancelFromPayPal", "PayPal", null),
                NotifyUrl = Url.AbsoluteAction("NotifyFromPayPal", "PayPal", null),
                CurrencyCode = settings.CurrencyCode
            };
            return View(model);
        }
    }
}