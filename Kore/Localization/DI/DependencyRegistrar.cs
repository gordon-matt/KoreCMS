using Autofac;
using Kore.Infrastructure;

namespace Kore.Localization.DI
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterModule<LocalizationModule>();
        }

        public int Order => 1;

        #endregion IDependencyRegistrar Members
    }
}