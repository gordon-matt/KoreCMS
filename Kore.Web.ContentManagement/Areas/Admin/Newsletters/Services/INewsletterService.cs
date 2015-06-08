using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.Localization;
using Kore.Security.Membership;
using Kore.Web.Events;
using Kore.Web.Security.Membership;

namespace Kore.Web.ContentManagement.Areas.Admin.Newsletters.Services
{
    public interface INewsletterService
    {
        bool Subscribe(string email, string name, KoreUser currentUser, out string message);
    }

    public class NewsletterService : INewsletterService
    {
        private readonly Lazy<IMembershipService> membershipService;
        private readonly Lazy<MembershipSettings> membershipSettings;
        private readonly Lazy<IEventBus> eventBus;
        private readonly Localizer T;

        public NewsletterService(Lazy<IMembershipService> membershipService)
        {
            this.membershipService = membershipService;
            T = NullLocalizer.Instance;
        }

        #region INewsletterService Members

        public bool Subscribe(string email, string name, KoreUser currentUser, out string message)
        {
            // First check if valid email address
            if (!CmsConstants.RegexPatterns.Email.IsMatch(email))
            {
                message = T(KoreWebLocalizableStrings.Membership.InvalidEmailAddress);
                return false;
            }

            var existingUser = membershipService.Value.GetUserByEmail(email);

            // Check if a user exists with that email..
            if (existingUser != null)
            {
                // if user is logged in already and is the same user with that email address
                if (currentUser != null && currentUser.Id == existingUser.Id)
                {
                    //auto set "ReceiveNewsletters" in profile to true
                    membershipService.Value.SaveProfileEntry(
                        currentUser.Id,
                        NewsletterUserProfileProvider.Fields.SubscribeToNewsletters,
                        bool.TrueString);

                    eventBus.Value.Notify<INewsletterEventHandler>(x => x.Subscribed(existingUser));

                    message = T(KoreCmsLocalizableStrings.Newsletters.SuccessfullySignedUp);
                    return true;
                }

                //else just tell user to login and set "ReceiveNewsletters" in profile to true
                message = T(KoreWebLocalizableStrings.Membership.UserEmailAlreadyExists);
                return false;
            }

            //create a user and email details to him/her with random password
            string password = System.Web.Security.Membership.GeneratePassword(
                membershipSettings.Value.GeneratedPasswordLength,
                membershipSettings.Value.GeneratedPasswordNumberOfNonAlphanumericChars);

            membershipService.Value.InsertUser(new KoreUser { UserName = email, Email = email }, password);
            var user = membershipService.Value.GetUserByEmail(email);

            // and sign up for newsletter, as requested.
            membershipService.Value.SaveProfileEntry(user.Id, NewsletterUserProfileProvider.Fields.SubscribeToNewsletters, bool.TrueString);

            name = name.Trim();
            if (name.Contains(" "))
            {
                string[] nameArray = name.Split(' ');
                string familyName = nameArray.Last();
                string givenNames = name.Replace(familyName, string.Empty).Trim();
                membershipService.Value.SaveProfileEntry(user.Id, AccountUserProfileProvider.Fields.FamilyName, familyName);
                membershipService.Value.SaveProfileEntry(user.Id, AccountUserProfileProvider.Fields.GivenNames, givenNames);
            }
            else
            {
                membershipService.Value.SaveProfileEntry(user.Id, AccountUserProfileProvider.Fields.GivenNames, name);
            }

            eventBus.Value.Notify<INewsletterEventHandler>(x => x.Subscribed(user));

            message = T(KoreCmsLocalizableStrings.Newsletters.SuccessfullySignedUp);
            return true;
        }

        #endregion
    }
}
