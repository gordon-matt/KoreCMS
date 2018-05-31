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

        public string Name => "CMS: Page Settings";

        public bool IsTenantRestricted => false;

        public string EditorTemplatePath => "Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Shared.EditorTemplates.PageSettings";

        #endregion ISettings Members

        [LocalizedDisplayName(KoreCmsLocalizableStrings.Settings.Pages.NumberOfPageVersionsToKeep)]
        public short NumberOfPageVersionsToKeep { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.Settings.Pages.ShowInvariantVersionIfLocalizedUnavailable)]
        public bool ShowInvariantVersionIfLocalizedUnavailable { get; set; }
    }
}