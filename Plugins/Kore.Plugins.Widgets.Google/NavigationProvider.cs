using Kore.Localization;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.Google
{
    public class NavigationProvider : INavigationProvider
    {
        public NavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(LocalizableStrings.Google), "99", BuildGoogleMenu);
        }

        private void BuildGoogleMenu(NavigationItemBuilder builder)
        {
            builder.IconCssClass("kore-icon kore-icon-google");

            builder.Add(T(LocalizableStrings.XMLSitemap), "5", item => item
                .Action("Index", "GoogleXmlSitemap", new { area = Constants.RouteArea })
                .IconCssClass("kore-icon kore-icon-sitemap")
                .Permission(StandardPermissions.FullAccess));

            // Later we can add Analytics integration, etc.
        }
    }
}