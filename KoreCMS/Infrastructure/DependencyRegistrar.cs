using System.Data.Entity;
using Autofac;
using Autofac.Core;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Data.SqlClient;
using Kore.EntityFramework.Data.EntityFramework;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Messaging;
using Kore.Web.Infrastructure;
using Kore.Web.Navigation;
using KoreCMS.Areas.Admin;
using KoreCMS.Messaging;
using KoreCMS.Services;

namespace KoreCMS.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<SqlDbHelper>().As<IKoreDbHelper>().SingleInstance();
            builder.RegisterType<SqlEntityFrameworkHelper>().As<IKoreEntityFrameworkHelper>().InstancePerDependency();

            var settings = DataSettingsManager.LoadSettings();
            builder.Register(x => settings).As<DataSettings>();

            // data layer

            #region Entity Framework 6

            builder.RegisterType<KoreCMS.Data.ApplicationDbContext>()
                .As<DbContext>()
                .AsSelf()
                .Named<DbContext>("KoreCMS.ApplicationDbContext")
                .WithParameter("nameOrConnectionString", settings.ConnectionString)
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EntityFrameworkRepository<>))
                .As(typeof(IRepository<>))
                .WithParameter(ResolvedParameter.ForNamed<DbContext>("KoreCMS.ApplicationDbContext"))
                .InstancePerLifetimeScope();

            #endregion Entity Framework 6

            #region Mongo DB

            //ConventionRegistry.Register("__kore_convention_pack__", KoreConventionPack.Instance, t => true);

            //builder.RegisterGeneric(typeof(MongoRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();

            //builder.Register(x =>
            //{
            //    var databaseName = MongoUrl.Create(settings.ConnectionString).DatabaseName;
            //    var mongoClient = new MongoClient(settings.ConnectionString);
            //    var client = mongoClient.GetServer();
            //    return client.GetDatabase(databaseName);
            //}).As<MongoDatabase>().SingleInstance();

            #endregion Mongo DB

            // SPA Routes
            builder.RegisterType<DurandalRouteProvider>().As<IDurandalRouteProvider>().SingleInstance();

            // services
            builder.RegisterType<MembershipService>().As<IMembershipService>().InstancePerDependency();

            // localization
            builder.RegisterType<LanguagePackInvariant>().As<ILanguagePack>().SingleInstance();

            // navigation
            builder.RegisterType<NavigationProvider>().As<INavigationProvider>().SingleInstance();

            // Messaging
            builder.RegisterType<AccountMessageTemplates>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AccountMessageTemplateTokensProvider>().As<IMessageTemplateTokensProvider>().SingleInstance();
        }

        public int Order
        {
            get { return 0; }
        }

        #endregion IDependencyRegistrar Members
    }
}