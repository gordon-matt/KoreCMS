using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kore.Web.Configuration
{
    public class KoreSiteSettings : ISettings
    {
        private string defaultLanguage;

        public KoreSiteSettings()
        {
            SiteName = "My Kore Site";
            DefaultGridPageSize = 10;
            DefaultLanguage = "en-US";
            DefaultFrontendLayoutPath = "~/Views/Shared/_Layout.cshtml";
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

        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "Default Frontend Layout Path")]
        public string DefaultFrontendLayoutPath { get; set; }

        [Display(Name = "Default Language")]
        public string DefaultLanguage
        {
            get { return string.IsNullOrEmpty(defaultLanguage) ? "en-US" : defaultLanguage; }
            set { defaultLanguage = value; }
        }

        [Display(Name = "Use Right-to-Left Layout")]
        public bool UseRightToLeft { get; set; }

        [Display(Name = "Default Grid Page Size")]
        public int DefaultGridPageSize { get; set; }
    }
}
