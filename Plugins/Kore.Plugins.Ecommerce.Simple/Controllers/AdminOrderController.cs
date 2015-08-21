using System.Web.Mvc;
using Kore.Web;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    [RoutePrefix("orders")]
    public class AdminOrderController : KoreController
    {
        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(SimpleCommercePermissions.ReadOrders))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store), Url.Action("Index", "AdminHome"));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Orders));

            ViewBag.Title = T(LocalizableStrings.Store);
            ViewBag.SubTitle = T(LocalizableStrings.Orders);

            return PartialView();
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            string json = string.Format(
@"{{
    View: '{0}',
    Columns: {{
        Id: '{1}',
        OrderDateUtc: '{2}',
        OrderTotal: '{3}',
        PaymentStatus: '{4}',
        Status: '{5}',
    }}
}}",
   T(KoreWebLocalizableStrings.General.View),
   T(LocalizableStrings.OrderModel.Id),
   T(LocalizableStrings.OrderModel.OrderDateUtc),
   T(LocalizableStrings.OrderModel.OrderTotal),
   T(LocalizableStrings.OrderModel.PaymentStatus),
   T(LocalizableStrings.OrderModel.Status));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}