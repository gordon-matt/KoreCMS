using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization
{
    public class LanguageSwitchBlock : ContentBlockBase
    {
        public enum LanguageSwitchStyle
        {
            BootstrapNavbarDropdown,
            Select,
            List,
            Custom
        }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.LanguageSwitchBlock.Style)]
        public LanguageSwitchStyle Style { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.LanguageSwitchBlock.UseUrlPrefix)]
        public bool UseUrlPrefix { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.LanguageSwitchBlock.CustomTemplatePath)]
        public string CustomTemplatePath { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Language Switch"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Localization.Views.Shared.DisplayTemplates.LanguageSwitchBlock"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Localization.Views.Shared.EditorTemplates.LanguageSwitchBlock"; }
        }

        #endregion ContentBlockBase Overrides
    }
}