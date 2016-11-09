using Autofac;
using Kore.Infrastructure;
using Kore.Localization;

namespace Kore.Web.Identity.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // localization
            builder.RegisterType<LanguagePackInvariant>().As<ILanguagePack>().SingleInstance();
        }

        public int Order
        {
            get { return 0; }
        }

        #endregion IDependencyRegistrar Members
    }
}