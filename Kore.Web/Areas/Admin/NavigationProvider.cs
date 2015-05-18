using Kore.Localization;
using Kore.Web.Areas.Admin.Configuration;
using Kore.Web.Areas.Admin.ScheduledTasks;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin
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
            // Configuration
            builder.Add(T(KoreWebLocalizableStrings.General.Configuration), "99", BuildConfigurationMenu);

            // Indexing
            builder.Add(T(KoreWebLocalizableStrings.General.Configuration),
                menu => menu.Add(T(KoreWebLocalizableStrings.Indexing.Title), "5", item => item
                    .Action("Index", "Indexing", new { area = KoreWebConstants.Areas.Indexing })
                    .IconCssClass("kore-icon kore-icon-search")
                    .Permission(StandardPermissions.FullAccess)));

            // Plugins
            builder.Add(T(KoreWebLocalizableStrings.General.Configuration),
                menu => menu.Add(T(KoreWebLocalizableStrings.Plugins.Title), "5", item => item
                    .Action("Index", "Plugin", new { area = KoreWebConstants.Areas.Plugins })
                    .IconCssClass("kore-icon kore-icon-plugins")
                    .Permission(StandardPermissions.FullAccess)));

            // Scheduled Tasks
            builder.Add(T(KoreWebLocalizableStrings.General.Configuration),
                menu => menu.Add(T(KoreWebLocalizableStrings.ScheduledTasks.Title), "5", item => item
                    .Action("Index", "ScheduledTask", new { area = KoreWebConstants.Areas.ScheduledTasks })
                    .IconCssClass("kore-icon kore-icon-schedule-tasks")
                    .Permission(ScheduledTasksPermissions.ReadScheduledTasks)));
        }

        private void BuildConfigurationMenu(NavigationItemBuilder builder)
        {
            builder.IconCssClass("kore-icon kore-icon-config");

            builder.Add(T(KoreWebLocalizableStrings.General.Settings), "5", item => item
                .Action("Index", "Settings", new { area = KoreWebConstants.Areas.Configuration })
                .IconCssClass("kore-icon kore-icon-settings")
                .Permission(ConfigurationPermissions.ReadSettings));

            builder.Add(T(KoreWebLocalizableStrings.General.Themes), "5", item => item
                .Action("Index", "Theme", new { area = KoreWebConstants.Areas.Configuration })
                .IconCssClass("kore-icon kore-icon-themes")
                .Permission(ConfigurationPermissions.ReadThemes));
        }
    }
}