using System;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization
{
    public class LanguageSwitchWidget : WidgetBase
    {
        public enum LanguageSwitchStyle
        {
            Select,
            List
        }

        #region WidgetBase Overrides

        public override string Name
        {
            get { return "Language Switch"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Localization.Views.Shared.DisplayTemplates.LanguageSwitchWidget"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Localization.Views.Shared.EditorTemplates.LanguageSwitchWidget"; }
        }

        #endregion WidgetBase Overrides

        public LanguageSwitchStyle Style { get; set; }

        public bool UseUrlPrefix { get; set; }

        public string MessageText { get; set; }
    }
}