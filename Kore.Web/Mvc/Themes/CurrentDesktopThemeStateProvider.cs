using System;
using Kore.Web.Configuration;
using Kore.Web.Environment;

namespace Kore.Web.Mvc.Themes
{
    public class CurrentDesktopThemeStateProvider : IWorkContextStateProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentDesktopThemeStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        #region IWorkContextStateProvider Members

        public Func<WorkContext, T> Get<T>(string name)
        {
            if (name == "CurrentDesktopTheme")
            {
                var currentTheme = KoreWebConfigurationSection.WebInstance.Themes.DefaultDesktopTheme;
                return ctx => (T)(object)currentTheme;
            }
            return null;
        }

        #endregion IWorkContextStateProvider Members
    }
}