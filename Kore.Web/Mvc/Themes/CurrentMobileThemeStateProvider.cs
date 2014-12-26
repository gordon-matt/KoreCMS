using System;
using Kore.Web.Configuration;
using Kore.Web.Environment;

namespace Kore.Web.Mvc.Themes
{
    public class CurrentMobileThemeStateProvider : IWorkContextStateProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentMobileThemeStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        #region IWorkContextStateProvider Members

        public Func<WorkContext, T> Get<T>(string name)
        {
            if (name == KoreWebConstants.StateProviders.CurrentMobileTheme)
            {
                var currentTheme = KoreWebConfigurationSection.WebInstance.Themes.DefaultMobileTheme;
                return ctx => (T)(object)currentTheme;
            }
            return null;
        }

        #endregion IWorkContextStateProvider Members
    }
}