using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Newsletters.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Newsletters)]
    [RoutePrefix("subscribers")]
    public class SubscriberController : KoreController
    {
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Newsletters.Title));
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Newsletters.Subscribers));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Newsletters.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Newsletters.Subscribers);

            return View("Kore.Web.ContentManagement.Areas.Admin.Newsletters.Views.Subscriber.Index");
        }
    }
}