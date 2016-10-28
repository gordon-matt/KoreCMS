using Autofac;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Plugins.Maintenance.Watchdog.Services;
using Kore.Web.Configuration;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Themes;
using Kore.Web.Navigation;
using Kore.Web.Plugins;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Maintenance.Watchdog.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar<ContainerBuilder> Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            if (!PluginManager.IsPluginInstalled(Constants.PluginSystemName))
            {
                return;
            }

            builder.RegisterType<DurandalRouteProvider>().As<IDurandalRouteProvider>().SingleInstance();
            builder.RegisterType<LanguagePackInvariant>().As<ILanguagePack>().SingleInstance();
            builder.RegisterType<NavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<LocationFormatProvider>().As<ILocationFormatProvider>().SingleInstance();
            builder.RegisterType<WatchdogSettings>().As<ISettings>().InstancePerLifetimeScope();
            builder.RegisterType<WebApiRegistrar>().As<IWebApiRegistrar>().SingleInstance();
            builder.RegisterType<WatchdogPermissions>().As<IPermissionProvider>().SingleInstance();
            builder.RegisterType<WatchdogInstanceService>().As<IWatchdogInstanceService>().InstancePerDependency();
        }

        public int Order
        {
            get { return 999; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}