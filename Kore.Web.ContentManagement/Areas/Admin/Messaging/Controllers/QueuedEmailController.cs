using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Messaging)]
    [RoutePrefix("queued-email")]
    public class QueuedEmailController : KoreController
    {
        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Messaging.Title));
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Messaging.QueuedEmails));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Messaging.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Messaging.QueuedEmails);

            return PartialView("Kore.Web.ContentManagement.Areas.Admin.Messaging.Views.QueuedEmail.Index");
        }
    }
}