using System;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization
{
    public class LanguageSwitchBlock : ContentBlockBase
    {
        public enum LanguageSwitchStyle
        {
            Select,
            List
        }

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

        public LanguageSwitchStyle Style { get; set; }

        public bool UseUrlPrefix { get; set; }

        public string MessageText { get; set; }
    }
}