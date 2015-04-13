using System.ComponentModel.DataAnnotations;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;

namespace Kore.Plugins.Widgets.Google.Widgets
{
    public class GoogleAdSenseWidget : WidgetBase
    {
        [Display(Name = "Google Ad Client")]
        public string AdClient { get; set; }

        [Display(Name = "Google Ad Slot")]
        public string AdSlot { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        [Display(Name = "Lazy Load")]
        public bool EnableLazyLoadAd { get; set; }

        #region WidgetBase Overrides

        public override string Name
        {
            get { return "Google: AdSense"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.Google/Views/Shared/DisplayTemplates/GoogleAdSenseWidget.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.Google/Views/Shared/EditorTemplates/GoogleAdSenseWidget.cshtml"; }
        }

        #endregion WidgetBase Overrides
    }
}