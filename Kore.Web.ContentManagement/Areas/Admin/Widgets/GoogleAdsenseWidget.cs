using System.ComponentModel.DataAnnotations;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets
{
    public class GoogleAdsenseWidget : WidgetBase
    {
        public override string Name
        {
            get { return "Google Adsense Widget"; }
        }

        [Display(Name = "Google Ad Client")]
        public string AdClient { get; set; }

        [Display(Name = "Google Ad Slot")]
        public string AdSlot { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        [Display(Name = "Lazy Load")]
        public bool EnableLazyLoadAd { get; set; }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Widgets.Views.Shared.DisplayTemplates.GoogleAdsenseWidget"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Widgets.Views.Shared.EditorTemplates.GoogleAdsenseWidget"; }
        }
    }
}