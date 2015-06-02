using Autofac;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Plugins.Widgets.FullCalendar.ContentBlocks;
using Kore.Plugins.Widgets.FullCalendar.Services;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Themes;
using Kore.Web.Navigation;
using Kore.Web.Plugins;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.FullCalendar.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar<ContainerBuilder> Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            if (!PluginManager.IsPluginInstalled("Kore.Plugins.Widgets.FullCalendar"))
            {
                return;
            }

            builder.RegisterType<LanguagePackInvariant>().As<ILanguagePack>().SingleInstance();

            builder.RegisterType<FullCalendarPermissions>().As<IPermissionProvider>().SingleInstance();
            builder.RegisterType<LocationFormatProvider>().As<ILocationFormatProvider>().SingleInstance();
            builder.RegisterType<NavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<WebApiRegistrar>().As<IWebApiRegistrar>().SingleInstance();

            builder.RegisterType<FullCalendarBlock>().As<IContentBlock>().InstancePerDependency();

            builder.RegisterType<CalendarService>().As<ICalendarService>().InstancePerDependency();
            builder.RegisterType<CalendarEventService>().As<ICalendarEventService>().InstancePerDependency();
        }

        public int Order
        {
            get { return 9999; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}