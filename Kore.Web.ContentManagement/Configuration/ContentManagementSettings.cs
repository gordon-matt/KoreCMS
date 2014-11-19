using System.ComponentModel.DataAnnotations;
using Kore.Web.Configuration;

namespace Kore.Web.ContentManagement.Configuration
{
    public class ContentManagementSettings : ISettings
    {
        private string defaultLanguage;

        public ContentManagementSettings()
        {
            SiteName = "Kore CMS";
            DefaultGridPageSize = 10;
            DefaultLanguage = "en-US";
        }

        #region ISettings Members

        [ScaffoldColumn(false)]
        public string Name
        {
            get { return "CMS Settings"; }
        }

        [ScaffoldColumn(false)]
        public string EditorTemplatePath
        {
            get { return "/Plugins/Kore.Web.ContentManagement/Views/Shared/EditorTemplates/ContentManagementSettings.cshtml"; }
        }

        #endregion ISettings Members

        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "Default Language")]
        public string DefaultLanguage
        {
            get { return string.IsNullOrEmpty(defaultLanguage) ? "en-US" : defaultLanguage; }
            set { defaultLanguage = value; }
        }

        [Display(Name = "Default Grid Page Size")]
        public int DefaultGridPageSize { get; set; }
    }
}