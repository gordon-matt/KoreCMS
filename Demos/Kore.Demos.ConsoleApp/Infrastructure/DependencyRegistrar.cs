using Autofac;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Data.SqlClient;
using Kore.Demos.ConsoleApp.Data;
using Kore.EntityFramework.Data.EntityFramework;
using Kore.Infrastructure;

namespace Kore.Demos.ConsoleApp.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        public int Order
        {
            get { return 1; }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<SqlDbHelper>().As<IKoreDbHelper>().SingleInstance();
            builder.RegisterType<SqlEntityFrameworkHelper>().As<IKoreEntityFrameworkHelper>().InstancePerDependency();

            builder.RegisterType<ApplicationDbContextFactory>().As<IDbContextFactory>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EntityFrameworkRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();
        }
    }
}