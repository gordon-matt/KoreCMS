using Autofac;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Plugins.Widgets.Google.Widgets;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;
using Kore.Web.Plugins;

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
            builder.RegisterType<GoogleAnalyticsWidget>().As<IWidget>().InstancePerDependency();
            builder.RegisterType<GoogleAdSenseWidget>().As<IWidget>().InstancePerDependency();
            builder.RegisterType<GoogleMapWidget>().As<IWidget>().InstancePerDependency();
        }

        public int Order
        {
            get { return 9999; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}