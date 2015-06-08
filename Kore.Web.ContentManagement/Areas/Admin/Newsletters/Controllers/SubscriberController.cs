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

        [AllowAnonymous]
        [Compress]
        [Route("subscribe")]
        public JsonResult Subscribe(string email, string name)
        {
            string message = string.Empty;
            bool success = newsletterService.Value.Subscribe(email, name, WorkContext.CurrentUser, out message);

            return Json(new
            {
                Successful = success,
                Message = message
            });

            //// First check if valid email address
            //if (!CmsConstants.RegexPatterns.Email.IsMatch(email))
            //{
            //    return Json(new
            //    {
            //        Successful = false,
            //        Message = T(KoreWebLocalizableStrings.Membership.InvalidEmailAddress).Text
            //    });
            //}

            //var existingUser = membershipService.Value.GetUserByEmail(email);

            //// Check if a user exists with that email..
            //if (existingUser != null)
            //{
            //    // if user is logged in already and is the same user with that email address
            //    if (WorkContext.CurrentUser != null && WorkContext.CurrentUser.Id == existingUser.Id)
            //    {
            //        //auto set "ReceiveNewsletters" in profile to true
            //        membershipService.Value.SaveProfileEntry(
            //            WorkContext.CurrentUser.Id,
            //            NewsletterUserProfileProvider.Fields.SubscribeToNewsletters,
            //            bool.TrueString);

            //        eventBus.Value.Notify<INewsletterEventHandler>(x => x.Subscribed(existingUser));

            //        return Json(new
            //        {
            //            Successful = true,
            //            Message = T(KoreCmsLocalizableStrings.Newsletters.SuccessfullySignedUp).Text
            //        });
            //    }

            //    //else just tell user to login and set "ReceiveNewsletters" in profile to true
            //    return Json(new
            //    {
            //        Successful = false,
            //        Message = T(KoreWebLocalizableStrings.Membership.UserEmailAlreadyExists).Text
            //    });
            //}

            ////create a user and email details to him/her with random password
            //string password = System.Web.Security.Membership.GeneratePassword(
            //    membershipSettings.Value.GeneratedPasswordLength,
            //    membershipSettings.Value.GeneratedPasswordNumberOfNonAlphanumericChars);

            //membershipService.Value.InsertUser(new KoreUser { UserName = email, Email = email }, password);
            //var user = membershipService.Value.GetUserByEmail(email);

            //// and sign up for newsletter, as requested.
            //membershipService.Value.SaveProfileEntry(user.Id, NewsletterUserProfileProvider.Fields.SubscribeToNewsletters, bool.TrueString);

            //name = name.Trim();
            //if (name.Contains(" "))
            //{
            //    string[] nameArray = name.Split(' ');
            //    string familyName = nameArray.Last();
            //    string givenNames = name.Replace(familyName, string.Empty).Trim();
            //    membershipService.Value.SaveProfileEntry(user.Id, AccountUserProfileProvider.Fields.FamilyName, familyName);
            //    membershipService.Value.SaveProfileEntry(user.Id, AccountUserProfileProvider.Fields.GivenNames, givenNames);
            //}
            //else
            //{
            //    membershipService.Value.SaveProfileEntry(user.Id, AccountUserProfileProvider.Fields.GivenNames, name);
            //}

            //eventBus.Value.Notify<INewsletterEventHandler>(x => x.Subscribed(user));

            //return Json(new
            //{
            //    Successful = true,
            //    Message = T(KoreCmsLocalizableStrings.Newsletters.SuccessfullySignedUp).Text
            //});
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