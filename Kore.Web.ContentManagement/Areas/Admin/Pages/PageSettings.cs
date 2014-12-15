using System.ComponentModel.DataAnnotations;
using Kore.Web.Configuration;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public class PageSettings : ISettings
    {
        public PageSettings()
        {
            NumberOfPageVersionsToKeep = 5;
        }

        #region ISettings Members

        [ScaffoldColumn(false)]
        public string Name
        {
            get { return "CMS: Page Settings"; }
        }

        public string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Shared.EditorTemplates.PageSettings"; }
        }

        #endregion

        [Display(Name = "# Page Versions to Keep")]
        public short NumberOfPageVersionsToKeep { get; set; }
    }
}