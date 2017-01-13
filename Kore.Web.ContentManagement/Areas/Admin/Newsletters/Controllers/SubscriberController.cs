using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kore.Collections;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Newsletters.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Security.Membership;

namespace Kore.Web.ContentManagement.Areas.Admin.Newsletters.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Newsletters)]
    [RoutePrefix("subscribers")]
    public class SubscriberController : KoreController
    {
        private readonly Lazy<IMembershipService> membershipService;
        private readonly Lazy<MembershipSettings> membershipSettings;
        private readonly Lazy<INewsletterService> newsletterService;

        public SubscriberController(
            Lazy<INewsletterService> newsletterService,
            Lazy<IMembershipService> membershipService,
            Lazy<MembershipSettings> membershipSettings)
        {
            this.newsletterService = newsletterService;
            this.membershipService = membershipService;
            this.membershipSettings = membershipSettings;
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
            return Json(new
            {
                Delete = T(KoreWebLocalizableStrings.General.Delete).Text,
                DeleteRecordConfirm = T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord).Text,
                DeleteRecordError = T(KoreWebLocalizableStrings.General.DeleteRecordError).Text,
                DeleteRecordSuccess = T(KoreWebLocalizableStrings.General.DeleteRecordSuccess).Text,
                Columns = new
                {
                    Email = T(KoreCmsLocalizableStrings.ContentBlocks.NewsletterSubscriptionBlock.Email).Text,
                    Name = T(KoreCmsLocalizableStrings.ContentBlocks.NewsletterSubscriptionBlock.Name).Text,
                }
            }, JsonRequestBehavior.AllowGet);
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
        public async Task<FileContentResult> DownloadCsv()
        {
            var userIds = (await membershipService.Value
                .GetProfileEntriesByKeyAndValue(WorkContext.CurrentTenant.Id, NewsletterUserProfileProvider.Fields.SubscribeToNewsletters, "true"))
                .Select(x => x.UserId);

            var users = (await membershipService.Value.GetUsers(WorkContext.CurrentTenant.Id, x => userIds.Contains(x.Id)))
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