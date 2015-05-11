using Kore.Localization;
using Kore.Web.Navigation;

namespace Kore.Web.Areas.Admin.ScheduledTasks
{
    public class ScheduledTasksNavigationProvider : INavigationProvider
    {
        public ScheduledTasksNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreWebLocalizableStrings.General.Configuration),
                menu => menu.Add(T(KoreWebLocalizableStrings.ScheduledTasks.Title), "5", item => item
                    .Action("Index", "ScheduledTask", new { area = KoreWebConstants.Areas.ScheduledTasks })
                    .IconCssClass("kore-icon kore-icon-schedule-tasks")
                    .Permission(ScheduledTasksPermissions.ReadScheduledTasks)));
        }
    }
}