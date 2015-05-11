using Autofac;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Plugins.Widgets.Google.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Themes;
using Kore.Web.Navigation;
using Kore.Web.Plugins;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.Google.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar<ContainerBuilder> Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            if (!PluginManager.IsPluginInstalled("Kore.Plugins.Widgets.Google"))
            {
                return;
            }

            builder.RegisterType<DefaultLocalizableStringsProvider>().As<IDefaultLocalizableStringsProvider>().SingleInstance();
            builder.RegisterType<GoogleAnalyticsBlock>().As<IContentBlock>().InstancePerDependency();
            builder.RegisterType<GoogleAdSenseBlock>().As<IContentBlock>().InstancePerDependency();
            builder.RegisterType<GoogleMapBlock>().As<IContentBlock>().InstancePerDependency();

            builder.RegisterType<GooglePermissions>().As<IPermissionProvider>().SingleInstance();
            builder.RegisterType<LocationFormatProvider>().As<ILocationFormatProvider>().SingleInstance();
            builder.RegisterType<NavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<WebApiRegistrar>().As<IWebApiRegistrar>().SingleInstance();
        }

        public int Order
        {
            get { return 9999; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}