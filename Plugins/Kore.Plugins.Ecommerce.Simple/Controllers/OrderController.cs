using System.Web.Mvc;
using Kore.Web.Mvc;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    [RoutePrefix("orders")]
    public class OrderController : KoreController
    {
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(SimpleCommercePermissions.ReadOrders))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Orders));

            ViewBag.Title = T(LocalizableStrings.Store);
            ViewBag.SubTitle = T(LocalizableStrings.Orders);

            return View();
        }
    }
}