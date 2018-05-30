using Kore.Localization;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership.Permissions;

namespace KoreCMS.Areas.Admin
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
            builder.Add(T(LocalizableStrings.Dashboard.Title), "0", BuildHomeMenu);
        }

        private static void BuildHomeMenu(NavigationItemBuilder builder)
        {
            builder.Permission(StandardPermissions.DashboardAccess);

            builder.IconCssClass("fa fa-home")
                .Url("#");
            //.Action("Index", "Home", new { area = KoreWebConstants.Areas.Admin });
        }
    }
}