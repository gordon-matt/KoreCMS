using Kore.Localization;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Newsletters
{
    public class NewslettersNavigationProvider : INavigationProvider
    {
        public NewslettersNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Newsletters.Subscribers), "5", item => item
                    .Action("Index", "Subscriber", new { area = Constants.Areas.Newsletters })
                    .IconCssClass("kore-icon kore-icon-subscribers")
                    .Permission(StandardPermissions.FullAccess)));
        }
    }
}