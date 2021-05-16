using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Kore.Collections;
using Kore.Web.Areas.Admin.Configuration.Models;
using Kore.Web.Configuration;
using Kore.Web.Mvc.Themes;
using Kore.Web.Security.Membership.Permissions;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;

namespace Kore.Web.Areas.Admin.Configuration.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class ThemeApiController : ODataController
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IThemeProvider themeProvider;
        private readonly IWorkContext workContext;
        private readonly KoreSiteSettings siteSettings;
        private readonly Lazy<ISettingService> settingsService;

        public ThemeApiController(
            IAuthorizationService authorizationService,
            IThemeProvider themeProvider,
            IWorkContext workContext,
            KoreSiteSettings siteSettings,
            Lazy<ISettingService> settingsService)
        {
            this.authorizationService = authorizationService;
            this.settingsService = settingsService;
            this.siteSettings = siteSettings;
            this.themeProvider = themeProvider;
            this.workContext = workContext;
        }

        // GET: odata/kore/cms/Plugins
        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IEnumerable<EdmThemeConfiguration> Get(ODataQueryOptions<EdmThemeConfiguration> options)
        {
            if (!CheckPermission(KoreWebPermissions.ThemesRead))
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

            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            };
            options.Validate(settings);

            var results = options.ApplyTo(themes.AsQueryable());
            return (results as IQueryable<EdmThemeConfiguration>).ToHashSet();
        }

        [HttpPost]
        public virtual void SetDesktopTheme(ODataActionParameters parameters)
        {
            // TODO: Change return type to IHttpResult and return UnauthorizedResult
            if (!CheckPermission(KoreWebPermissions.ThemesWrite))
            {
                return;
            }

            string themeName = (string)parameters["themeName"];
            var themeConfig = themeProvider.GetThemeConfiguration(themeName);

            siteSettings.DefaultDesktopTheme = themeName;

            if (!string.IsNullOrEmpty(themeConfig.DefaultLayoutPath))
            {
                siteSettings.DefaultFrontendLayoutPath = themeConfig.DefaultLayoutPath;
            }
            else
            {
                siteSettings.DefaultFrontendLayoutPath = "~/Views/Shared/_Layout.cshtml";
            }

            settingsService.Value.SaveSettings(siteSettings, workContext.CurrentTenant.Id);
            KoreWebConstants.ResetCache();
        }

        [HttpPost]
        public virtual void SetMobileTheme(ODataActionParameters parameters)
        {
            // TODO: Change return type to IHttpResult and return UnauthorizedResult
            if (!CheckPermission(KoreWebPermissions.ThemesWrite))
            {
                return;
            }

            string themeName = (string)parameters["themeName"];
            var themeConfig = themeProvider.GetThemeConfiguration(themeName);

            siteSettings.DefaultMobileTheme = themeName;

            if (!string.IsNullOrEmpty(themeConfig.DefaultLayoutPath))
            {
                siteSettings.DefaultFrontendLayoutPath = themeConfig.DefaultLayoutPath;
            }
            else
            {
                siteSettings.DefaultFrontendLayoutPath = "~/Views/Shared/_Layout.cshtml";
            }

            settingsService.Value.SaveSettings(siteSettings, workContext.CurrentTenant.Id);
            KoreWebConstants.ResetCache();
        }

        protected bool CheckPermission(Permission permission)
        {
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }
    }
}