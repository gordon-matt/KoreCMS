using System.ComponentModel.DataAnnotations;
using Kore.ComponentModel;

namespace Kore.Web.Configuration
{
    public class KoreSiteSettings : ISettings
    {
        //private string defaultLanguage;

        public KoreSiteSettings()
        {
            SiteName = "My Kore Site";
            DefaultDesktopTheme = "Default";
            DefaultMobileTheme = "Default";
            DefaultGridPageSize = 10;
            //DefaultLanguage = "en-US";
            DefaultFrontendLayoutPath = "~/Views/Shared/_Layout.cshtml";
            AdminLayoutPath = "~/Areas/Admin/Views/Shared/_Layout.cshtml";
            HomePageTitle = "Home Page";
        }

        #region ISettings Members

        [ScaffoldColumn(false)]
        public string Name
        {
            get { return "Kore Site Settings"; }
        }

        [ScaffoldColumn(false)]
        public string EditorTemplatePath
        {
            get { return "Kore.Web.Views.Shared.EditorTemplates.KoreSiteSettings"; }
        }

        #endregion ISettings Members

        #region General

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Site.SiteName)]
        public string SiteName { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Site.DefaultFrontendLayoutPath)]
        public string DefaultFrontendLayoutPath { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Site.AdminLayoutPath)]
        public string AdminLayoutPath { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Site.DefaultGridPageSize)]
        public int DefaultGridPageSize { get; set; }

        #endregion General

        #region Themes

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Site.DefaultDesktopTheme)]
        public string DefaultDesktopTheme { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Site.DefaultMobileTheme)]
        public string DefaultMobileTheme { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Site.AllowUserToSelectTheme)]
        public bool AllowUserToSelectTheme { get; set; }

        #endregion Themes

        //#region Localization

        //[LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Site.DefaultLanguage)]
        //public string DefaultLanguage
        //{
        //    //get { return string.IsNullOrEmpty(defaultLanguage) ? "en-US" : defaultLanguage; }
        //    get { return defaultLanguage; }
        //    set { defaultLanguage = value; }
        //}

        //#endregion Localization

        #region SEO

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Site.DefaultMetaKeywords)]
        public string DefaultMetaKeywords { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Site.DefaultMetaDescription)]
        public string DefaultMetaDescription { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Site.HomePageTitle)]
        public string HomePageTitle { get; set; }

        #endregion SEO
    }
}