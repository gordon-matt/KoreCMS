using Autofac;
using Kore.Infrastructure;
using Kore.Web.Infrastructure;

namespace Kore.Web.CommonResources.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ResourceBundleRegistrar>().As<IResourceBundleRegistrar>().SingleInstance();
            builder.RegisterType<RequireJSConfigProvider>().As<IRequireJSConfigProvider>().SingleInstance();
        }

        public int Order
        {
            get { return 1; }
        }

        #endregion IDependencyRegistrar Members
    }
}