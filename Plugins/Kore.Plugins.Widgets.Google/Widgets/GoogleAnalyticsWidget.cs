using Kore.Web.ContentManagement.Areas.Admin.Widgets;

namespace Kore.Plugins.Widgets.Google.Widgets
{
    public class GoogleAnalyticsWidget : WidgetBase
    {
        public string AccountNumber { get; set; }

        public string DomainName { get; set; }

        #region WidgetBase Overrides

        public override string Name
        {
            get { return "Google: Analytics"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.Google/Views/Shared/DisplayTemplates/GoogleAnalyticsWidget.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.Google/Views/Shared/EditorTemplates/GoogleAnalyticsWidget.cshtml"; }
        }

        #endregion WidgetBase Overrides
    }
}