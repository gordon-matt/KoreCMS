using Kore.Localization;
using Kore.Web.Navigation;

namespace Kore.Web.ContentManagement.Messaging
{
    public class MessagingNavigationProvider : INavigationProvider
    {
        public MessagingNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Messaging.MessageTemplates), "5", item => item
                    .Action("Index", "MessageTemplate", new { area = Constants.Areas.Messaging })
                    .IconCssClass("kore-icon kore-icon-message-templates")
                    .Permission(Permissions.ManageMessages)));

            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Messaging.QueuedEmails), "5", item => item
                    .Action("Index", "QueuedEmail", new { area = Constants.Areas.Messaging })
                    .IconCssClass("kore-icon kore-icon-message-queue")
                    .Permission(Permissions.ManageMessages)));
        }
    }
}