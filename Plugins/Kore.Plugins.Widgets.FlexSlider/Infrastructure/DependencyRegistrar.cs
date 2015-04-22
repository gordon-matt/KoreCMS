using Autofac;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Plugins.Widgets.FlexSlider.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Themes;
using Kore.Web.Navigation;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.FlexSlider.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar<ContainerBuilder> Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            if (!PluginManager.IsPluginInstalled("Kore.Plugins.Widgets.FlexSlider"))
            {
                return;
            }

            builder.RegisterType<DefaultLocalizableStringsProvider>().As<IDefaultLocalizableStringsProvider>().SingleInstance();
            builder.RegisterType<FlexSliderBlock>().As<IContentBlock>().InstancePerDependency();

            builder.RegisterType<LocationFormatProvider>().As<ILocationFormatProvider>().SingleInstance();
        }

        public int Order
        {
            get { return 9999; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}