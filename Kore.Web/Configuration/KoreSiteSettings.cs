using System.ComponentModel.DataAnnotations;

namespace Kore.Web.Configuration
{
    public class KoreSiteSettings : ISettings
    {
        private string defaultLanguage;

        public KoreSiteSettings()
        {
            SiteName = "My Kore Site";
            DefaultDesktopTheme = "Default";
            DefaultMobileTheme = "Default";
            DefaultGridPageSize = 10;
            DefaultLanguage = "en-US";
            DefaultFrontendLayoutPath = "~/Views/Shared/_Layout.cshtml";
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

        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "Default Frontend Layout Path")]
        public string DefaultFrontendLayoutPath { get; set; }

        [Display(Name = "Default Grid Page Size")]
        public int DefaultGridPageSize { get; set; }

        #endregion General

        #region Themes

        [Display(Name = "Default Desktop Theme")]
        public string DefaultDesktopTheme { get; set; }

        [Display(Name = "Default Mobile Theme")]
        public string DefaultMobileTheme { get; set; }

        [Display(Name = "Allow User To Select Theme")]
        public bool AllowUserToSelectTheme { get; set; }

        #endregion Themes

        #region Localization

        [Display(Name = "Default Language")]
        public string DefaultLanguage
        {
            get { return string.IsNullOrEmpty(defaultLanguage) ? "en-US" : defaultLanguage; }
            set { defaultLanguage = value; }
        }

        [Display(Name = "Use Right-to-Left Layout")]
        public bool UseRightToLeft { get; set; }

        #endregion Localization

        #region SEO

        [Display(Name = "Default Meta Keywords")]
        public string DefaultMetaKeywords { get; set; }

        [Display(Name = "Default Meta Description")]
        public string DefaultMetaDescription { get; set; }

        [Display(Name = "Home Page Title")]
        public string HomePageTitle { get; set; }

        #endregion SEO
    }
}