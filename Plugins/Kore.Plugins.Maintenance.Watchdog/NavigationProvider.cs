using Kore.Localization;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Maintenance.Watchdog
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
            builder.Add(T(Kore.Web.KoreWebLocalizableStrings.Maintenance.Title),
                menu => menu.Add(T(LocalizableStrings.Services), "5", item => item
                    .Action("Index", "Watchdog", new { area = Constants.RouteArea })
                    .IconCssClass("kore-icon kore-icon-services")
                    .Permission(StandardPermissions.FullAccess)));
        }
    }
}