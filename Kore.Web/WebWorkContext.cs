using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kore.Exceptions;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Tenants;
using Kore.Tenants.Domain;
using Kore.Tenants.Services;
using Kore.Web.Mvc.Notify;
using Kore.Web.Navigation;

namespace Kore.Web
{
    public partial class WebWorkContext : IWebWorkContext
    {
        private Tenant cachedTenant;

        private readonly ITenantService tenantService;
        private readonly IWebHelper webHelper;
        private readonly ConcurrentDictionary<string, Func<object>> stateResolvers = new ConcurrentDictionary<string, Func<object>>();
        private readonly IEnumerable<IWorkContextStateProvider> workContextStateProviders;

        public WebWorkContext()
        {
            tenantService = EngineContext.Current.Resolve<ITenantService>();
            webHelper = EngineContext.Current.Resolve<IWebHelper>();
            workContextStateProviders = EngineContext.Current.ResolveAll<IWorkContextStateProvider>();
            Breadcrumbs = new BreadcrumbCollection();
            Notifications = new List<NotifyEntry>();
        }

        #region IWorkContext Members

        public T GetState<T>(string name)
        {
            var resolver = stateResolvers.GetOrAdd(name, FindResolverForState<T>);
            return (T)resolver();
        }

        public void SetState<T>(string name, T value)
        {
            stateResolvers[name] = () => value;
        }

        public HttpContextBase HttpContext
        {
            get { return GetState<HttpContextBase>("HttpContext"); }
        }

        public BreadcrumbCollection Breadcrumbs { get; set; }

        public ICollection<NotifyEntry> Notifications { get; set; }

        public string CurrentDesktopTheme
        {
            get { return GetState<string>(KoreWebConstants.StateProviders.CurrentDesktopTheme); }
            set { SetState(KoreWebConstants.StateProviders.CurrentDesktopTheme, value); }
        }

        public string CurrentMobileTheme
        {
            get { return GetState<string>(KoreWebConstants.StateProviders.CurrentMobileTheme); }
        }

        public string CurrentCultureCode
        {
            get { return GetState<string>(KoreWebConstants.StateProviders.CurrentCultureCode); }
        }

        public KoreUser CurrentUser
        {
            get { return GetState<KoreUser>(KoreWebConstants.StateProviders.CurrentUser); }
        }

        public virtual Tenant CurrentTenant
        {
            get
            {
                if (cachedTenant != null)
                {
                    return cachedTenant;
                }

                // Try to determine the current tenant by HTTP_HOST
                var host = webHelper.ServerVariables("HTTP_HOST");
                var allTenants = tenantService.Find();
                var tenant = allTenants.FirstOrDefault(s => s.ContainsHostValue(host));

                if (tenant == null)
                {
                    // Load the first found tenant
                    tenant = allTenants.FirstOrDefault();
                }
                if (tenant == null)
                {
                    throw new KoreException("No tenant could be loaded");
                }

                cachedTenant = tenant;
                return cachedTenant;
            }
        }

        #endregion IWorkContext Members

        private Func<object> FindResolverForState<T>(string name)
        {
            var resolver = workContextStateProviders.Select(wcsp => wcsp.Get<T>(name)).FirstOrDefault(value => !Equals(value, default(T)));

            if (resolver == null)
            {
                return () => default(T);
            }
            return () => resolver(this);
        }
    }
}