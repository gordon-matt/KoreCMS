using Autofac;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Plugins.Widgets.Bootstrap3.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.Bootstrap3.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar<ContainerBuilder> Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            if (!PluginManager.IsPluginInstalled("Kore.Plugins.Widgets.Bootstrap3"))
            {
                return;
            }

            builder.RegisterType<LanguagePackInvariant>().As<ILanguagePack>().SingleInstance();
            builder.RegisterType<Bootstrap3CarouselBlock>().As<IContentBlock>().InstancePerDependency();
            builder.RegisterType<Bootstrap3ImageGalleryBlock>().As<IContentBlock>().InstancePerDependency();
        }

        public int Order
        {
            get { return 9999; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}