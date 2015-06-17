using System;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using Kore.Configuration;
using Kore.Infrastructure;
using Kore.Tasks;
using Kore.Web.Hosting;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc;
using Kore.Web.Mvc.EmbeddedViews;
using Kore.Web.Mvc.Filters;
using Kore.Web.Mvc.Themes;

namespace Kore.Web
{
    public abstract class HttpApplicationBase : HttpApplication
    {
        protected virtual void OnBeforeApplicationStart()
        {
        }

        protected void Application_Start()
        {
            //initialize engine context
            EngineContext.Default = new KoreWebEngine();
            EngineContext.Initialize(false);

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("Content/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("Media/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("Scripts/{*pathInfo}");
            //RouteTable.Routes.IgnoreRoute("Styles/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("Images/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("favicon.ico");

            //we use our own mobile devices support (".Mobile" is reserved). that's why we disable it.
            var mobileDisplayMode = DisplayModeProvider.Instance.Modes
                .FirstOrDefault(x => x.DisplayModeId == DisplayModeProvider.MobileDisplayModeId);

            if (mobileDisplayMode != null)
            {
                DisplayModeProvider.Instance.Modes.Remove(mobileDisplayMode);
            }

            //initialize engine context
            //EngineContext.Initialize(false);
            OnBeforeApplicationStart();

            //set dependency resolver
            var dependencyResolver = new KoreDependencyResolver();
            DependencyResolver.SetResolver(dependencyResolver);

            if (DataSettingsHelper.IsDatabaseInstalled)
            {
                //remove all view engines
                ViewEngines.Engines.Clear();
                //except the themeable razor view engine we use
                ViewEngines.Engines.Add(new ThemeableRazorViewEngine());
            }

            //register virtual path provider for embedded views
            var embeddedResourceResolver = EngineContext.Current.Resolve<IEmbeddedResourceResolver>();

            var embeddedViewProvider = new EmbeddedViewVirtualPathProvider(embeddedResourceResolver.Views);
            HostingEnvironment.RegisterVirtualPathProvider(embeddedViewProvider);

            var embeddedScriptsProvider = new EmbeddedScriptVirtualPathProvider(embeddedResourceResolver.Scripts);
            HostingEnvironment.RegisterVirtualPathProvider(embeddedScriptsProvider);

            var embeddedStylesProvider = new EmbeddedStyleVirtualPathProvider(embeddedResourceResolver.Styles);
            HostingEnvironment.RegisterVirtualPathProvider(embeddedStylesProvider);

            //
            // Register Virtual Path Providers
            //
            if (HostingEnvironment.IsHosted)
            {
                foreach (var vpp in EngineContext.Current.ResolveAll<IKoreVirtualPathProvider>())
                {
                    HostingEnvironment.RegisterVirtualPathProvider(vpp.Instance);
                }
            }

            FilterProviders.Providers.Add(new KoreFilterProvider());

            if (DataSettingsHelper.IsDatabaseInstalled && KoreConfigurationSection.Instance.Tasks.Enabled)
            {
                TaskManager.Instance.Initialize();
                TaskManager.Instance.Start();
            }

            OnApplicationStart();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            OnApplicationEnd();
        }

        protected virtual void OnApplicationEnd()
        {
        }

        protected virtual void OnApplicationStart()
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //ignore static resources
            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            if (webHelper.IsStaticResource(this.Request))
            {
                return;
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //ignore static resources
            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            if (webHelper.IsStaticResource(this.Request))
            {
                return;
            }
        }
    }
}