using System;
using Kore.Infrastructure;
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

        public Func<IWebWorkContext, T> Get<T>(string name)
        {
            if (name == KoreWebConstants.StateProviders.CurrentDesktopTheme)
            {
                //var currentTheme = KoreWebConfigurationSection.WebInstance.Themes.DefaultDesktopTheme;
                string currentTheme = EngineContext.Current.Resolve<IThemeContext>().WorkingDesktopTheme;
                return ctx => (T)(object)currentTheme;
            }
            return null;
        }

        #endregion IWorkContextStateProvider Members
    }
}