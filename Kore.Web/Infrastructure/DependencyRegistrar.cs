using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Kore.Caching;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Logging;
using Kore.Net.Mail;
using Kore.Tasks.Services;
using Kore.Web.Areas.Admin.Configuration;
using Kore.Web.Areas.Admin.Indexing;
using Kore.Web.Areas.Admin.Plugins;
using Kore.Web.Areas.Admin.ScheduledTasks;
using Kore.Web.Configuration;
using Kore.Web.Environment;
using Kore.Web.Events;
using Kore.Web.Fakes;
using Kore.Web.Hosting;
using Kore.Web.Indexing;
using Kore.Web.Indexing.Services;
using Kore.Web.IO.FileSystems.AppData;
using Kore.Web.IO.FileSystems.LockFile;
using Kore.Web.IO.FileSystems.VirtualPath;
using Kore.Web.Localization;
using Kore.Web.Localization.Services;
using Kore.Web.Mobile;
using Kore.Web.Mvc.EmbeddedViews;
using Kore.Web.Mvc.Notify;
using Kore.Web.Mvc.Resources;
using Kore.Web.Mvc.RoboUI;
using Kore.Web.Mvc.Routing;
using Kore.Web.Mvc.Themes;
using Kore.Web.Navigation;
using Kore.Web.Plugins;
using Kore.Web.Security.Membership;
using Kore.Web.Security.Membership.Permissions;
using IFilterProvider = Kore.Web.Mvc.Filters.IFilterProvider;

namespace Kore.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            #region HTTP context and other related stuff

            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.Register(RequestContextFactory).As<RequestContext>().InstancePerDependency();
            builder.Register(UrlHelperFactory).As<UrlHelper>().InstancePerDependency();

            #endregion HTTP context and other related stuff

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            #region Controllers

            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());
            builder.RegisterApiControllers(typeFinder.GetAssemblies().ToArray());

            #endregion Controllers

            //plugins
            builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerLifetimeScope();

            //cache manager
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("Kore_Cache_Static").SingleInstance();
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("Kore_Cache_Per_Request").InstancePerLifetimeScope();

            //work context, themes, routes, etc
            builder.RegisterType<WebWorkContext>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<MobileDeviceHelper>().As<IMobileDeviceHelper>().InstancePerLifetimeScope();
            builder.RegisterType<ThemeProvider>().As<IThemeProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ThemeContext>().As<IThemeContext>().InstancePerLifetimeScope();

            builder.RegisterType<EmbeddedResourceResolver>().As<IEmbeddedResourceResolver>().SingleInstance();
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();

            builder.Register(ctx => RouteTable.Routes).SingleInstance();
            builder.Register(ctx => ModelBinders.Binders).SingleInstance();
            builder.Register(ctx => ViewEngines.Engines).SingleInstance();

            builder.RegisterType<ResourcesManager>().As<IResourcesManager>().InstancePerLifetimeScope();
            builder.RegisterType<RolesBasedAuthorizationService>().As<IAuthorizationService>().SingleInstance();

            builder.RegisterType<DefaultEventBus>().As<IEventBus>().SingleInstance();

            // configuration
            builder.RegisterModule<ConfigurationModule>();
            builder.RegisterType<DefaultSettingService>().As<ISettingService>();
            builder.RegisterType<ScheduledTaskService>().As<IScheduledTaskService>().InstancePerDependency();
            builder.RegisterType<KoreSiteSettings>().As<ISettings>().SingleInstance();
            builder.RegisterType<SmtpSettings>().As<ISettings>().SingleInstance();
            builder.RegisterType<CaptchaSettings>().As<ISettings>().SingleInstance();

            //builder.RegisterType<SearchSettings>().As<ISettings>().SingleInstance();

            builder.RegisterModule<LoggingModule>();

            // navigation
            builder.RegisterType<NavigationManager>().As<INavigationManager>().InstancePerDependency();
            builder.RegisterType<ConfigurationNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<IndexingNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<PluginsNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<ScheduledTasksNavigationProvider>().As<INavigationProvider>().SingleInstance();

            // permission providers
            builder.RegisterType<StandardPermissions>().As<IPermissionProvider>();
            builder.RegisterType<ConfigurationPermissions>().As<IPermissionProvider>().SingleInstance();
            builder.RegisterType<ScheduledTasksPermissions>().As<IPermissionProvider>().SingleInstance();

            // work context state providers
            builder.RegisterType<CurrentUserStateProvider>().As<IWorkContextStateProvider>();
            builder.RegisterType<CurrentDesktopThemeStateProvider>().As<IWorkContextStateProvider>();
            builder.RegisterType<CurrentMobileThemeStateProvider>().As<IWorkContextStateProvider>();
            builder.RegisterType<CurrentCultureCodeStateProvider>().As<IWorkContextStateProvider>();
            builder.RegisterType<HttpContextStateProvider>().As<IWorkContextStateProvider>();

            // localization
            builder.RegisterType<DefaultLocalizableStringsProvider>().As<IDefaultLocalizableStringsProvider>().SingleInstance();
            builder.RegisterType<LanguageManager>().As<ILanguageManager>().SingleInstance();
            builder.RegisterType<DefaultLocalizedStringManager>().As<ILocalizedStringManager>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultWebCultureManager>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<SiteCultureSelector>().As<ICultureSelector>().SingleInstance();
            builder.RegisterType<CookieCultureSelector>().As<ICultureSelector>().SingleInstance();

            // misc
            builder.RegisterType<WebApiRegistrar>().As<IWebApiRegistrar>().SingleInstance();
            builder.RegisterType<DefaultEmailSender>().As<IEmailSender>().InstancePerDependency();
            builder.RegisterType<RoboUIVirtualPathProvider>().As<IKoreVirtualPathProvider>().SingleInstance();

            // file systems
            builder.RegisterType<AppDataFolder>().As<IAppDataFolder>().SingleInstance();
            builder.RegisterType<AppDataFolderRoot>().As<IAppDataFolderRoot>().SingleInstance();
            builder.RegisterType<DefaultVirtualPathMonitor>().As<IVirtualPathMonitor>().SingleInstance();
            builder.RegisterType<Clock>().As<IClock>().SingleInstance();
            builder.RegisterType<DefaultLockFileManager>().As<ILockFileManager>().SingleInstance();

            // indexing
            builder.RegisterType<DefaultIndexManager>().As<IIndexManager>().InstancePerDependency();
            builder.RegisterType<IndexingService>().As<IIndexingService>().SingleInstance();
            builder.RegisterType<IndexingTaskExecutor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<IndexNotifierHandler>().As<IIndexNotifierHandler>().InstancePerDependency();
            builder.RegisterType<SearchService>().As<ISearchService>().InstancePerDependency();

            builder.RegisterType<Notifier>().As<INotifier>().InstancePerDependency();
            builder.RegisterType<NotifyFilter>().As<IFilterProvider>().InstancePerLifetimeScope();

            // user profile providers
            builder.RegisterType<AccountUserProfileProvider>().As<IUserProfileProvider>().SingleInstance();
            builder.RegisterType<LocalizationUserProfileProvider>().As<IUserProfileProvider>().SingleInstance();
            builder.RegisterType<MobileUserProfileProvider>().As<IUserProfileProvider>().SingleInstance();
        }

        private static RequestContext RequestContextFactory(IComponentContext context)
        {
            var httpContextAccessor = context.Resolve<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.Current();
            if (httpContext != null)
            {
                var mvcHandler = httpContext.Handler as MvcHandler;
                if (mvcHandler != null)
                {
                    return mvcHandler.RequestContext;
                }

                var hasRequestContext = httpContext.Handler as IHasRequestContext;
                if (hasRequestContext != null)
                {
                    if (hasRequestContext.RequestContext != null)
                        return hasRequestContext.RequestContext;
                }
            }
            else
            {
                httpContext = new HttpContextPlaceholder();
            }

            return new RequestContext(httpContext, new RouteData());
        }

        private static UrlHelper UrlHelperFactory(IComponentContext context)
        {
            return new UrlHelper(context.Resolve<RequestContext>(), context.Resolve<RouteCollection>());
        }

        public int Order
        {
            get { return 100; }
        }

        /// <summary>
        /// standin context for background tasks.
        /// </summary>
        private class HttpContextPlaceholder : HttpContextBase
        {
            public override HttpRequestBase Request
            {
                get { return new HttpRequestPlaceholder(); }
            }

            public override IHttpHandler Handler { get; set; }
        }

        /// <summary>
        /// standin context for background tasks.
        /// </summary>
        private class HttpRequestPlaceholder : HttpRequestBase
        {
            /// <summary>
            /// anonymous identity provided for background task.
            /// </summary>
            public override bool IsAuthenticated
            {
                get { return false; }
            }

            // empty collection provided for background operation
            public override NameValueCollection Form
            {
                get
                {
                    return new NameValueCollection();
                }
            }
        }
    }
}