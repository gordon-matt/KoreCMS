using Autofac;
using Kore.Indexing.Services;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;
using Kore.Web.Indexing;
using Kore.Web.Navigation;

namespace Kore.Indexing.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar<ContainerBuilder> Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<LuceneIndexProvider>().As<IIndexProvider>().InstancePerDependency();
            builder.RegisterType<LuceneSearchBuilder>().As<ISearchBuilder>().InstancePerDependency();
            builder.RegisterType<NavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<SearchWidget>().As<IWidget>().InstancePerDependency();
        }

        public int Order
        {
            get { return 9999; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}