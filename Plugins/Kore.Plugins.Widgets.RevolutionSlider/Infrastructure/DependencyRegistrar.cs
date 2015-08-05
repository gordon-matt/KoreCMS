using Autofac;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Plugins.Widgets.RevolutionSlider.ContentBlocks;
using Kore.Plugins.Widgets.RevolutionSlider.Services;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Themes;
using Kore.Web.Navigation;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RevolutionSlider.Infrastructure
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
            builder.RegisterType<RevolutionSliderBlock>().As<IContentBlock>().InstancePerDependency();

            builder.RegisterType<WebApiRegistrar>().As<IWebApiRegistrar>().SingleInstance();

            builder.RegisterType<SliderService>().As<ISliderService>().InstancePerDependency();
            builder.RegisterType<SlideService>().As<ISlideService>().InstancePerDependency();
            builder.RegisterType<LayerService>().As<ILayerService>().InstancePerDependency();
        }

        public int Order
        {
            get { return 9999; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}