using Autofac;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Web.Configuration;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Themes;
using Kore.Web.Navigation;
using Kore.Web.Plugins;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Watchdog.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar<ContainerBuilder> Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            if (!PluginManager.IsPluginInstalled("Kore.Plugins.Watchdog"))
            {
                return;
            }

            builder.RegisterType<DefaultLocalizableStringsProvider>().As<IDefaultLocalizableStringsProvider>().SingleInstance();
            builder.RegisterType<NavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<LocationFormatProvider>().As<ILocationFormatProvider>().SingleInstance();
            builder.RegisterType<WatchdogSettings>().As<ISettings>().SingleInstance();
            builder.RegisterType<WebApiRegistrar>().As<IWebApiRegistrar>().SingleInstance();
            builder.RegisterType<WatchdogPermissions>().As<IPermissionProvider>().SingleInstance();
        }

        public int Order
        {
            get { return 999; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}