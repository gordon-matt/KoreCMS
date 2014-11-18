using Kore.Localization;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Plugins
{
    public class PluginsNavigationProvider : INavigationProvider
    {
        public PluginsNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreWebLocalizableStrings.General.Configuration),
                menu => menu.Add(T(KoreWebLocalizableStrings.Plugins.Title), "5", item => item
                    .Action("Index", "Plugin", new { area = KoreWebConstants.Areas.Plugins })
                    .IconCssClass("kore-icon kore-icon-plugins")
                    .Permission(StandardPermissions.FullAccess)));
        }
    }
}