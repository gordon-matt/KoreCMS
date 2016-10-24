using System;
using System.Linq;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Threading;
using Kore.Web.Configuration;

namespace Kore.Web.Mvc.Themes
{
    /// <summary>
    /// Theme context
    /// </summary>
    public partial class ThemeContext : IThemeContext
    {
        private readonly IWorkContext workContext;
        private readonly IThemeProvider themeProvider;
        private readonly KoreSiteSettings siteSettings;

        private bool isDesktopThemeCached;
        private string cachedDesktopThemeName;

        private bool isMobileThemeCached;
        private string cachedMobileThemeName;

        public ThemeContext(
            IWorkContext workContext,
            IThemeProvider themeProvider,
            KoreSiteSettings siteSettings)
        {
            this.workContext = workContext;
            this.themeProvider = themeProvider;
            this.siteSettings = siteSettings;
        }

        /// <summary>
        /// Get or set current theme for desktops
        /// </summary>
        public string WorkingDesktopTheme
        {
            get
            {
                if (isDesktopThemeCached)
                {
                    return cachedDesktopThemeName;
                }

                string theme = string.Empty;

                if (siteSettings.AllowUserToSelectTheme)
                {
                    if (workContext.CurrentUser != null)
                    {
                        var membershipService = EngineContext.Current.Resolve<IMembershipService>();
                        string userTheme = AsyncHelper.RunSync(() => membershipService.GetProfileEntry(workContext.CurrentUser.Id, ThemeUserProfileProvider.Fields.PreferredTheme));

                        if (!string.IsNullOrEmpty(userTheme))
                        {
                            theme = userTheme;
                        }
                    }
                }

                // Default tenant theme
                if (string.IsNullOrEmpty(theme))
                {
                    theme = siteSettings.DefaultDesktopTheme ?? "Default";
                }

                // Ensure that theme exists
                if (!themeProvider.ThemeConfigurationExists(theme))
                {
                    var themeInstance = themeProvider.GetThemeConfigurations()
                        .FirstOrDefault(x => !x.MobileTheme);

                    if (themeInstance == null)
                    {
                        throw new Exception("No desktop theme could be loaded");
                    }

                    theme = themeInstance.ThemeName;
                }

                // Cache theme
                this.cachedDesktopThemeName = theme;
                this.isDesktopThemeCached = true;
                return theme;
            }
            set
            {
                if (!siteSettings.AllowUserToSelectTheme)
                {
                    return;
                }

                if (workContext.CurrentUser == null)
                {
                    return;
                }

                var membershipService = EngineContext.Current.Resolve<IMembershipService>();
                AsyncHelper.RunSync(() => membershipService.SaveProfileEntry(workContext.CurrentUser.Id, ThemeUserProfileProvider.Fields.PreferredTheme, value));

                //clear cache
                this.isDesktopThemeCached = false;
            }
        }

        /// <summary>
        /// Get current theme for mobile (e.g. Mobile)
        /// </summary>
        public string WorkingMobileTheme
        {
            get
            {
                if (isMobileThemeCached)
                    return cachedMobileThemeName;

                // Default store theme
                string theme = siteSettings.DefaultMobileTheme;

                // Ensure that theme exists
                if (!themeProvider.ThemeConfigurationExists(theme))
                {
                    var themeInstance = themeProvider.GetThemeConfigurations().FirstOrDefault(x => x.MobileTheme);

                    if (themeInstance == null)
                    {
                        throw new Exception("No mobile theme could be loaded");
                    }

                    theme = themeInstance.ThemeName;
                }

                //cache theme
                this.cachedMobileThemeName = theme;
                this.isMobileThemeCached = true;
                return theme;
            }
        }
    }
}