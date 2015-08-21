using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kore.Collections;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Newsletters.Services;
using Kore.Web.Events;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Security.Membership;
using Kore.Web.Security.Membership.Permissions;
using Newtonsoft.Json.Linq;

namespace Kore.Web.ContentManagement.Areas.Admin.Newsletters.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Newsletters)]
    [RoutePrefix("subscribers")]
    public class SubscriberController : KoreController
    {
        private readonly Lazy<IMembershipService> membershipService;
        private readonly Lazy<MembershipSettings> membershipSettings;
        private readonly Lazy<IEventBus> eventBus;
        private readonly Lazy<INewsletterService> newsletterService;

        public SubscriberController(
            Lazy<INewsletterService> newsletterService,
            Lazy<IMembershipService> membershipService,
            Lazy<MembershipSettings> membershipSettings,
            Lazy<IEventBus> eventBus)
        {
            this.newsletterService = newsletterService;
            this.membershipService = membershipService;
            this.membershipSettings = membershipSettings;
            this.eventBus = eventBus;
        }

        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(CmsPermissions.NewsletterRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Newsletters.Title));
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Newsletters.Subscribers));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Newsletters.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Newsletters.Subscribers);

            return PartialView("Kore.Web.ContentManagement.Areas.Admin.Newsletters.Views.Subscriber.Index");
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            string json = string.Format(
@"{{
    Delete: '{0}',
    DeleteRecordConfirm: '{1}',
    DeleteRecordError: '{2}',
    DeleteRecordSuccess: '{3}',
    Columns: {{
        Email: '{4}',
        Name: '{5}',
    }}
}}",
   T(KoreWebLocalizableStrings.General.Delete),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreCmsLocalizableStrings.ContentBlocks.NewsletterSubscriptionBlock.Email),
   T(KoreCmsLocalizableStrings.ContentBlocks.NewsletterSubscriptionBlock.Name));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [Compress]
        [Route("subscribe")]
        [ValidateAntiForgeryToken]
        public JsonResult Subscribe(string email, string name)
        {
            string message = string.Empty;
            bool success = newsletterService.Value.Subscribe(email, name, WorkContext.CurrentUser, out message);

            return Json(new
            {
                Success = success,
                Message = message
            });
        }

        [Compress]
        [Route("download-csv")]
        public FileContentResult DownloadCsv()
        {
            var userIds = membershipService.Value
                .GetProfileEntriesByKeyAndValue(NewsletterUserProfileProvider.Fields.SubscribeToNewsletters, "true")
                .Select(x => x.UserId);

            var users = membershipService.Value.GetUsers(x => userIds.Contains(x.Id))
                .ToHashSet()
                .Select(x => new
                {
                    Email = x.Email,
                    Name = membershipService.Value.GetUserDisplayName(x)
                })
                .OrderBy(x => x.Name);

            string csv = users.ToCsv();
            string fileName = string.Format("Subscribers_{0:yyyy_MM_dd}.csv", DateTime.Now);
            return File(new UTF8Encoding().GetBytes(csv), "text/csv", fileName);
        }
    }
}