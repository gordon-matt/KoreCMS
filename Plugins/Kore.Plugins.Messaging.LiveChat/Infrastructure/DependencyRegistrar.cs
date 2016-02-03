using Autofac;
using Kore.Infrastructure;
using Kore.Plugins.Messaging.LiveChat.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.Mvc.Themes;
using Kore.Web.Plugins;

namespace Kore.Plugins.Messaging.LiveChat.Infrastructure
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

            builder.RegisterType<OwinStartupConfiguration>().As<IOwinStartupConfiguration>().InstancePerDependency();
            builder.RegisterType<LiveChatBlock>().As<IContentBlock>().InstancePerDependency();

            //builder.RegisterType<DurandalRouteProvider>().As<IDurandalRouteProvider>().SingleInstance();

            //builder.RegisterType<LanguagePackInvariant>().As<ILanguagePack>().SingleInstance();

            builder.RegisterType<LocationFormatProvider>().As<ILocationFormatProvider>().SingleInstance();
            //builder.RegisterType<NavigationProvider>().As<INavigationProvider>().SingleInstance();
        }

        public int Order
        {
            get { return 9999; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}