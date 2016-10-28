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
            builder.Add(T(KoreWebLocalizableStrings.Membership.Title), "1", BuildMembershipMenu);
            builder.Add(T(KoreWebLocalizableStrings.General.Configuration), "3", BuildConfigurationMenu);
            builder.Add(T(KoreWebLocalizableStrings.Maintenance.Title), "4", BuildMaintenanceMenu);
            builder.Add(T(KoreWebLocalizableStrings.Plugins.Title), "99999", BuildPluginsMenu);
        }

        private void BuildMembershipMenu(NavigationItemBuilder builder)
        {
            builder
                .Url("#membership")
                .IconCssClass("kore-icon kore-icon-membership")
                .Permission(StandardPermissions.FullAccess);
        }

        private void BuildConfigurationMenu(NavigationItemBuilder builder)
        {
            builder.IconCssClass("kore-icon kore-icon-config");

            // Tenants
            builder.Add(T(KoreWebLocalizableStrings.Tenants.Title), "5", item => item
                .Url("#tenants")
                .IconCssClass("kore-icon kore-icon-tenants")
                .Permission(StandardPermissions.FullAccess));

            // Indexing
            builder.Add(T(KoreWebLocalizableStrings.Indexing.Title), "5", item => item
                .Url("#indexing")
                .IconCssClass("kore-icon kore-icon-search")
                .Permission(StandardPermissions.FullAccess));

            // Plugins
            builder.Add(T(KoreWebLocalizableStrings.Plugins.Title), "5", item => item
                .Url("#plugins")
                .IconCssClass("kore-icon kore-icon-plugins")
                .Permission(StandardPermissions.FullAccess));

            // Settings
            builder.Add(T(KoreWebLocalizableStrings.General.Settings), "5", item => item
                .Url("#configuration/settings")
                .IconCssClass("kore-icon kore-icon-settings")
                .Permission(ConfigurationPermissions.ReadSettings));

            // Themes
            builder.Add(T(KoreWebLocalizableStrings.General.Themes), "5", item => item
                .Url("#configuration/themes")
                .IconCssClass("kore-icon kore-icon-themes")
                .Permission(ConfigurationPermissions.ReadThemes));
        }

        private void BuildMaintenanceMenu(NavigationItemBuilder builder)
        {
            builder.IconCssClass("kore-icon kore-icon-maintenance");

            builder.Add(T(KoreWebLocalizableStrings.Log.Title), "5", item => item
                .Url("#log")
                .IconCssClass("kore-icon kore-icon-log")
                .Permission(StandardPermissions.FullAccess));

            // Scheduled Tasks
            builder.Add(T(KoreWebLocalizableStrings.ScheduledTasks.Title), "5", item => item
                .Url("#scheduledtasks")
                .IconCssClass("kore-icon kore-icon-schedule-tasks")
                .Permission(ScheduledTasksPermissions.ReadScheduledTasks));
        }

        private void BuildPluginsMenu(NavigationItemBuilder builder)
        {
            builder.IconCssClass("kore-icon kore-icon-plugins");
        }
    }
}