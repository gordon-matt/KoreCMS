using Kore.Localization;
using Kore.Web.Navigation;

namespace Kore.Web.Areas.Admin.Configuration
{
    public class ConfigurationNavigationProvider : INavigationProvider
    {
        public ConfigurationNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreWebLocalizableStrings.General.Configuration), "99", BuildConfigurationMenu);
        }

        private void BuildConfigurationMenu(NavigationItemBuilder builder)
        {
            builder.IconCssClass("kore-icon kore-icon-config");

            builder.Add(T(KoreWebLocalizableStrings.General.Settings), "5", item => item
                .Action("Index", "Settings", new { area = KoreWebConstants.Areas.Configuration })
                .IconCssClass("kore-icon kore-icon-settings")
                .Permission(ConfigurationPermissions.ManageSettings));

            builder.Add(T(KoreWebLocalizableStrings.General.Themes), "5", item => item
                .Action("Index", "Theme", new { area = KoreWebConstants.Areas.Configuration })
                .IconCssClass("kore-icon kore-icon-themes")
                .Permission(ConfigurationPermissions.ManageThemes));
        }
    }
}