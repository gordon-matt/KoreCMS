using System.ComponentModel.DataAnnotations;
using Kore.ComponentModel;
using Kore.Web.Configuration;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public class PageSettings : ISettings
    {
        public PageSettings()
        {
            NumberOfPageVersionsToKeep = 5;
            ShowInvariantVersionIfLocalizedUnavailable = true;
        }

        #region ISettings Members

        public string Name
        {
            get { return "CMS: Page Settings"; }
        }

        public bool IsTenantRestricted
        {
            get { return false; }
        }

        public string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Shared.EditorTemplates.PageSettings"; }
        }

        #endregion ISettings Members

        [LocalizedDisplayName(KoreCmsLocalizableStrings.Settings.Pages.NumberOfPageVersionsToKeep)]
        public short NumberOfPageVersionsToKeep { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.Settings.Pages.ShowInvariantVersionIfLocalizedUnavailable)]
        public bool ShowInvariantVersionIfLocalizedUnavailable { get; set; }
    }
}