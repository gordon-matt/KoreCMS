using System.Web.Mvc;
using Kore.Web;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Plugins.Widgets.RevolutionSlider.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    public class SliderController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(Permissions.Read))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Plugins.Title));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.RevolutionSlider));

            ViewBag.Title = T(LocalizableStrings.RevolutionSlider);

            return PartialView();
        }
    }
}