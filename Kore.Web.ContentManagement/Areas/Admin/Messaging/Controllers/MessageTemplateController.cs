using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Messaging.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Messaging)]
    [RoutePrefix("templates")]
    public class MessageTemplateController : KoreController
    {
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Messaging.Title));
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Messaging.MessageTemplates));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Messaging.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Messaging.MessageTemplates);

            return View("Kore.Web.ContentManagement.Areas.Admin.Messaging.Views.MessageTemplate.Index");
        }
    }
}