using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.Mvc.Notify;
using Kore.Web.Navigation;

namespace Kore.Web
{
    public partial class WorkContext : IWorkContext
    {
        private readonly ConcurrentDictionary<string, Func<object>> stateResolvers = new ConcurrentDictionary<string, Func<object>>();
        private readonly IEnumerable<IWorkContextStateProvider> workContextStateProviders;

        public WorkContext()
        {
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

        public string ShortDatePattern
        {
            get
            {
                return string.Format("{{0:{0}}}",
                    Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern);
            }
        }

        public string FullDateTimePattern
        {
            get
            {
                return string.Format("{{0:{0} {1}}}",
                    Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern,
                    Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern);
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