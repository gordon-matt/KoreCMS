using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Kore.Infrastructure;
using Kore.Web.Areas.Admin.Configuration.Models;
using Kore.Web.Configuration;
using Kore.Web.Mvc.Themes;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Configuration.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class ThemeApiController : ODataController
    {
        private readonly IThemeProvider themeProvider;
        private readonly KoreSiteSettings siteSettings;
        private readonly Lazy<ISettingService> settingsService;

        public ThemeApiController(
            IThemeProvider themeProvider,
            KoreSiteSettings siteSettings,
            Lazy<ISettingService> settingsService)
        {
            this.themeProvider = themeProvider;
            this.siteSettings = siteSettings;
            this.settingsService = settingsService;
        }

        // GET: odata/kore/cms/Plugins
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IQueryable<EdmThemeConfiguration> Get()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<EdmThemeConfiguration>().AsQueryable();
            }

            var themes = themeProvider.GetThemeConfigurations()
                .Select(x => (EdmThemeConfiguration)x)
                .ToList();

            foreach (var theme in themes)
            {
                if (theme.Title == siteSettings.DefaultDesktopTheme)
                {
                    theme.IsDefaultDesktopTheme = true;
                }
                if (theme.Title == siteSettings.DefaultMobileTheme)
                {
                    theme.IsDefaultMobileTheme = true;
                }
            }

            return themes.AsQueryable();
        }

        [HttpPost]
        public virtual void SetDesktopTheme(ODataActionParameters parameters)
        {
            // TODO: Change return type to IHttpResult and return UnauthorizedResult
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return;
            }

            string themeName = (string)parameters["themeName"];
            siteSettings.DefaultDesktopTheme = themeName;
            settingsService.Value.SaveSettings(siteSettings);
        }

        [HttpPost]
        public virtual void SetMobileTheme(ODataActionParameters parameters)
        {
            // TODO: Change return type to IHttpResult and return UnauthorizedResult
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return;
            }

            string themeName = (string)parameters["themeName"];
            siteSettings.DefaultMobileTheme = themeName;
            settingsService.Value.SaveSettings(siteSettings);
        }

        protected static bool CheckPermission(Permission permission)
        {
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }
    }
}