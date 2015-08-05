using Autofac;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Services;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Themes;
using Kore.Web.Navigation;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Infrastructure
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
            builder.RegisterType<LocationFormatProvider>().As<ILocationFormatProvider>().SingleInstance();
            builder.RegisterType<NavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<RoyalVideoPlayerBlock>().As<IContentBlock>().InstancePerDependency();
            builder.RegisterType<WebApiRegistrar>().As<IWebApiRegistrar>().SingleInstance();

            builder.RegisterType<PlaylistService>().As<IPlaylistService>().InstancePerDependency();
            builder.RegisterType<PlaylistVideoService>().As<IPlaylistVideoService>().InstancePerDependency();
            builder.RegisterType<VideoService>().As<IVideoService>().InstancePerDependency();
        }

        public int Order
        {
            get { return 9999; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}