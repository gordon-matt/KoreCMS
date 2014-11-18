using System;
using System.Linq;
using Kore.Web.Configuration;

namespace Kore.Web.Mvc.Themes
{
    /// <summary>
    /// Theme context
    /// </summary>
    public partial class ThemeContext : IThemeContext
    {
        private readonly IThemeProvider _themeProvider;

        private bool _desktopThemeIsCached;
        private string _cachedDesktopThemeName;

        private bool _mobileThemeIsCached;
        private string _cachedMobileThemeName;

        public ThemeContext(IThemeProvider themeProvider)
        {
            this._themeProvider = themeProvider;
        }

        /// <summary>
        /// Get or set current theme for desktops
        /// </summary>
        public string WorkingDesktopTheme
        {
            get
            {
                if (_desktopThemeIsCached)
                    return _cachedDesktopThemeName;

                string theme = "";
                if (KoreWebConfigurationSection.WebInstance.Themes.AllowUserToSelectTheme)
                {
                    //if (_workContext.CurrentCustomer != null)
                    //    theme = _workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.WorkingDesktopThemeName, _genericAttributeService, _storeContext.CurrentStore.Id);
                }

                //default store theme
                if (string.IsNullOrEmpty(theme))
                    theme = KoreWebConfigurationSection.WebInstance.Themes.DefaultDesktopTheme;

                //ensure that theme exists
                if (!_themeProvider.ThemeConfigurationExists(theme))
                {
                    var themeInstance = _themeProvider.GetThemeConfigurations()
                        .FirstOrDefault(x => !x.MobileTheme);
                    if (themeInstance == null)
                        throw new Exception("No desktop theme could be loaded");
                    theme = themeInstance.ThemeName;
                }

                //cache theme
                this._cachedDesktopThemeName = theme;
                this._desktopThemeIsCached = true;
                return theme;
            }
            set
            {
                if (!KoreWebConfigurationSection.WebInstance.Themes.AllowUserToSelectTheme)
                    return;

                //clear cache
                this._desktopThemeIsCached = false;
            }
        }

        /// <summary>
        /// Get current theme for mobile (e.g. Mobile)
        /// </summary>
        public string WorkingMobileTheme
        {
            get
            {
                if (_mobileThemeIsCached)
                    return _cachedMobileThemeName;

                //default store theme
                string theme = KoreWebConfigurationSection.WebInstance.Themes.DefaultMobileTheme;

                //ensure that theme exists
                if (!_themeProvider.ThemeConfigurationExists(theme))
                {
                    var themeInstance = _themeProvider.GetThemeConfigurations().FirstOrDefault(x => x.MobileTheme);

                    if (themeInstance == null)
                    {
                        throw new Exception("No mobile theme could be loaded");
                    }

                    theme = themeInstance.ThemeName;
                }

                //cache theme
                this._cachedMobileThemeName = theme;
                this._mobileThemeIsCached = true;
                return theme;
            }
        }
    }
}