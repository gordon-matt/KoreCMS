using System.Data.Entity;
using Autofac;
using Autofac.Core;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Messaging;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership;
using KoreCMS.Areas.Admin;
using KoreCMS.Messaging;
using KoreCMS.Security.Membership;
using KoreCMS.Services;

namespace KoreCMS.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // data layer
            builder.RegisterType<KoreCMS.Data.ApplicationDbContext>()
                .As<DbContext>()
                .AsSelf()
                .Named<DbContext>("KoreCMS.ApplicationDbContext")
                .WithParameter("nameOrConnectionString", "DefaultConnection")
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EntityFrameworkRepository<>))
                .As(typeof(IRepository<>))
                .WithParameter(ResolvedParameter.ForNamed<DbContext>("KoreCMS.ApplicationDbContext"))
                .InstancePerLifetimeScope();

            // services
            builder.RegisterType<MembershipService>().As<IMembershipService>().InstancePerDependency();

            // localization
            builder.RegisterType<DefaultLocalizableStringsProvider>().As<IDefaultLocalizableStringsProvider>().SingleInstance();

            // navigation
            builder.RegisterType<NavigationProvider>().As<INavigationProvider>().SingleInstance();

            // Messaging
            builder.RegisterType<AccountMessageTemplates>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AccountMessageTemplateTokensProvider>().As<IMessageTemplateTokensProvider>().SingleInstance();

            builder.RegisterType<UserProfileProvider>().As<IUserProfileProvider>().SingleInstance();
        }

        public int Order
        {
            get { return 0; }
        }

        #endregion IDependencyRegistrar Members
    }
}