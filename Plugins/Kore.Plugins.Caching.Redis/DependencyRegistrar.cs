using Autofac;
using Kore.Caching;
using Kore.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Caching.Redis
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            if (!PluginManager.IsPluginInstalled(Constants.PluginSystemName))
            {
                return;
            }

            builder.RegisterType<RedisCacheManager>().As<ICacheManager>().Named<ICacheManager>("Kore_Cache_Static").SingleInstance();
            builder.RegisterType<RedisCacheManager>().As<ICacheManager>().Named<ICacheManager>("Kore_Cache_Per_Request").InstancePerLifetimeScope();
        }

        public int Order => 99999;
    }
}