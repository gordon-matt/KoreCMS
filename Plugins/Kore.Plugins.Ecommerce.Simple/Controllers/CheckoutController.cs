using System.Web.Mvc;
using Kore.Web.Mvc;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [RouteArea("")]
    [RoutePrefix("store/checkout")]
    public class CheckoutController : KoreController
    {
        private readonly StoreSettings settings;

        public CheckoutController(StoreSettings settings)
        {
            this.settings = settings;
        }

        [Route("")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Checkout));

            ViewBag.Title = T(LocalizableStrings.Store);
            ViewBag.SubTitle = T(LocalizableStrings.Checkout);

            ViewBag.CurrencyCode = settings.Currency;
            return View();
        }

        [Route("completed/{orderId}")]
        public ActionResult Completed(int orderId)
        {
            return View(orderId);
        }
    }
}