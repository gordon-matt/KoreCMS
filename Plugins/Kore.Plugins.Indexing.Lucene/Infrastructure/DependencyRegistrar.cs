using Autofac;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Plugins.Indexing.Lucene.Services;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.Indexing;
using Kore.Web.Plugins;

namespace Kore.Plugins.Indexing.Lucene.Infrastructure
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

            builder.RegisterType<LanguagePackInvariant>().As<ILanguagePack>().SingleInstance();

            builder.RegisterType<LuceneIndexProvider>().As<IIndexProvider>().InstancePerDependency();
            builder.RegisterType<LuceneSearchBuilder>().As<ISearchBuilder>().InstancePerDependency();
            builder.RegisterType<LuceneSearchBlock>().As<IContentBlock>().InstancePerDependency();
        }

        public int Order
        {
            get { return 9999; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}