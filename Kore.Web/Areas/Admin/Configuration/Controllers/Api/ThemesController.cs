using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Kore.Web.Areas.Admin.Configuration.Models;
using Kore.Web.Configuration;
using Kore.Web.Mvc.Themes;

namespace Kore.Web.Areas.Admin.Configuration.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class ThemesController : ODataController
    {
        private readonly IThemeProvider themeProvider;
        private readonly KoreSiteSettings siteSettings;
        private readonly Lazy<ISettingService> settingsService;

        public ThemesController(
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
            var themes = themeProvider.GetThemeConfigurations()
                .Select(x => (EdmThemeConfiguration)x)
                .ToList();

            foreach (var theme in themes)
            {
                if (theme.Title == siteSettings.DefaultDesktopTheme)
                {
                    theme.IsDefaultDesktopTheme = true;
                }

                //TODO
                //if (theme.Title == themeContext.WorkingMobileTheme)
                //{
                //}
            }

            return themes.AsQueryable();
        }

        [HttpPost]
        public virtual void SetDesktopTheme(ODataActionParameters parameters)
        {
            string themeName = (string)parameters["themeName"];
            siteSettings.DefaultDesktopTheme = themeName;
            settingsService.Value.SaveSettings(siteSettings);
        }
    }
}